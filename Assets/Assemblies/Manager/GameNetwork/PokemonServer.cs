using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Google.Protobuf;
using MemoryPack;
using Mirror;
using Proto;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    // using userId = String;
    [Serializable, MemoryPackable]
    public partial class PlayerRecord
    {
        public string userId;
        public PlayerData data;
        public PackageData package;
        public Vector3 position;
    }

    [Serializable]
    public class PokemonServer : MonoSingleton<PokemonServer>
    {
        public const int DefaultTeamId = 0;
        protected override bool DontDestroyOnLoad() => false;

        // 玩家记录
        [SerializeField] private SerializableDictionary<string, PlayerRecord> records;

        // 非持久化数据
        private Dictionary<string, NetworkConnectionToClient> _onlineId2Conn;

        private Dictionary<NetworkConnectionToClient, string> _onLineConn2Id;

        private Dictionary<string, Player> _id2PlayerController;


        protected override void OnInit()
        {
            _onlineId2Conn = new Dictionary<string, NetworkConnectionToClient>();
            _onLineConn2Id = new Dictionary<NetworkConnectionToClient, string>();
            _id2PlayerController = new Dictionary<string, Player>();
            records = new SerializableDictionary<string, PlayerRecord>();
        }

        protected override void OnDispose()
        {
        }

        public void AddPlayer(Player controller)
        {
            _id2PlayerController.Add(controller.userId, controller);
        }

        public void RemovePlayer(Player controller)
        {
            _id2PlayerController.Remove(controller.userId);
        }

        [Server]
        public IEnumerator<Player> OnlinePlayers()
        {
            // TODO Error Handle
            foreach (var controller in _id2PlayerController.Values)
            {
                yield return controller;
            }
        }


        [Server]
        public bool IsOnline(string userId)
        {
            return _onlineId2Conn.ContainsKey(userId);
        }


        [Server]
        public async UniTask<bool> UserLogin(string userId, NetworkConnectionToClient conn)
        {
            _onlineId2Conn.Add(userId, conn);
            _onLineConn2Id.Add(conn, userId);

            // Download
            var response = await GrpcClient.GameService.DownloadPlayerDataAsync(new DownloadRequest
            {
                Uid = userId
            }).ResponseAsync;
            if (response.Error.Code != StatusCode.Ok)
            {
                Debug.LogError(response.Error.Content);
                return false;
            }
            records[userId] = MemoryPackSerializer.Deserialize<PlayerRecord>(response.Data.Span);
            return true; // TODO 
        }

        [Server]
        public async void UserOffLine(string userId)
        {
            NetworkServer.DestroyPlayerForConnection(_onlineId2Conn[userId]);
            _onLineConn2Id.Remove(_onlineId2Conn[userId]);
            _onlineId2Conn.Remove(userId);

            Debug.Log(records[userId].data.teamId);
            // Upload
            ErrorMessage errorMessage = await GrpcClient.GameService.UploadPlayerDataAsync(new UploadRequest
            {
                Uid = userId,
                Data = ByteString.CopyFrom(MemoryPackSerializer.Serialize(records[userId]))
            }).ResponseAsync;
            if (errorMessage.Code != StatusCode.Ok)
            {
                Debug.LogError(errorMessage.Content);
            }
        }

        [Server]
        public bool QueryConnection(string userId, out NetworkConnectionToClient conn)
        {
            return _onlineId2Conn.TryGetValue(userId, out conn);
        }

        [Server]
        public void QueryId(NetworkConnectionToClient conn, out string userId)
        {
            _onLineConn2Id.TryGetValue(conn, out userId);
        }


        [Server]
        public void UserOffLine(NetworkConnectionToClient conn)
        {
            string userId = _onLineConn2Id[conn];
            UserOffLine(userId);
        }

        [Server]
        public async UniTask<bool> Register(string userId, string playerName)
        {
            // 没有这个用户 可以注册
            ErrorMessage registerResponse = await GrpcClient.GameService.RegisterUserAsync(new RegisterRequest
            {
                Uid = userId,
                Name = playerName
            }).ResponseAsync;
            if (registerResponse.Code != StatusCode.Ok)
            {
                Debug.LogError(registerResponse.Content);
                return false;
            }

            // TODO 多样初始化
            var record = new PlayerRecord
            {
                userId = userId,
                data = new PlayerData(userId, playerName, DefaultTeamId, new PokemonData(PokemonEnum.玩家)),
                package = new PackageData(),
                position = Vector3.zero,
            };
            records[userId] = record;

            ErrorMessage errorMessage = await GrpcClient.GameService.UploadPlayerDataAsync(new UploadRequest()
            {
                Uid = userId,
                Data = ByteString.CopyFrom(MemoryPackSerializer.Serialize(record))
            });
            if (errorMessage.Code != StatusCode.Ok)
            {
                Debug.LogError(errorMessage.Content);
                return false;
            }

            return true;
        }

        [Server]
        public async UniTask<bool> ContainsUser(string userId, string playerName)
        {
            ErrorMessage containsResponse = await GrpcClient.GameService.ContainsUserAsync(new UserInfo()
            {
                Uid = userId,
                Name = playerName
            }).ResponseAsync;
            if (containsResponse.Code == StatusCode.Ok) return true;
            return false;
        }

        [Server]
        public bool QueryPlayerData(string userId, out PlayerData data)
        {
            if (records.TryGetValue(userId, out var record))
            {
                data = record.data;
                return true;
            }

            data = null;
            return false;
        }

        [Server]
        public bool QueryPackageData(string userId, out PackageData data)
        {
            if (records.TryGetValue(userId, out var record))
            {
                data = record.package;
                return true;
            }

            data = null;
            return false;
        }

        [Server]
        public bool QueryPosition(string userId, out Vector3 position)
        {
            if (_id2PlayerController.TryGetValue(userId, out var player) && player.state == Player.NetworkState.Ready)
            {
                position = player.controller.position;
                return true;
            }

            if (records.TryGetValue(userId, out var record))
            {
                position = record.position;
                return true;
            }

            position = Vector3.zero;
            return false;
        }

        [Server]
        public void UpdatePosition(string userId, Vector3 pokemonPosition)
        {
            if (records.TryGetValue(userId, out var record))
            {
                record.position = pokemonPosition;
            }
        }
        // [Server]
        // public void QueryGroupData(int id, out TeamGroup teamGroup)
        // {
        //     if (!id2TeamGroup.TryGetValue(id, out teamGroup))
        //     {
        //         teamGroup = TeamGroup.Default;
        //         Debug.LogError($"Group:{id} not found , use default group");
        //     }
        // }
    }
}