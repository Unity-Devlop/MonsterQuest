using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using UnityToolkit;

namespace Game
{
    [Serializable]
    public class PlayerRecord
    {
        public string userId;
        public PlayerData data;
        public PackageData package;
        public Vector3 position;
        public TeamGroup teamGroup;
    }

    [Serializable]
    public class PokemonServer : MonoSingleton<PokemonServer>
    {
        protected override bool DontDestroyOnLoad() => false;

        private static readonly string RecordPath = "Record.json";
        private static readonly string TeamGroupPath = "TeamGroup.json";

        // 玩家记录
        [SerializeField] private SerializableDictionary<string, PlayerRecord> records;
        [SerializeField] private SerializableDictionary<int, TeamGroup> id2TeamGroup;

        
        // 非持久化数据
        private Dictionary<string, NetworkConnectionToClient> _onlineId2Conn;

        private Dictionary<NetworkConnectionToClient, string> _onLineConn2Id;

        private Dictionary<string, Player> _id2PlayerController;


        protected override void OnInit()
        {
            _onlineId2Conn = new Dictionary<string, NetworkConnectionToClient>();
            _onLineConn2Id = new Dictionary<NetworkConnectionToClient, string>();
            _id2PlayerController = new Dictionary<string, Player>();
            Load();
        }

        protected override void OnDispose()
        {
            Save();
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
        [Sirenix.OdinInspector.Button]
        private void Save()
        {
            JsonUtil.SaveJsonToStreamingAssets(RecordPath, records);
            JsonUtil.SaveJsonToStreamingAssets(TeamGroupPath, id2TeamGroup);
        }

        [Server]
        [Sirenix.OdinInspector.Button]
        private void Load()
        {
            records = JsonUtil.LoadJsonFromStreamingAssets<SerializableDictionary<string, PlayerRecord>>(RecordPath);

            id2TeamGroup = JsonUtil.LoadJsonFromStreamingAssets<SerializableDictionary<int, TeamGroup>>(TeamGroupPath);
            // DataBase = JsonUtil.LoadJsonFromStreamingAssets<PokemonDataBase>(DataBasePath);


            if (records == null) records = new SerializableDictionary<string, PlayerRecord>();
            if (id2TeamGroup == null)
            {
                id2TeamGroup = new SerializableDictionary<int, TeamGroup>
                {
                    // preset groups
                    [0] = TeamGroup.Default,
                    [1] = new TeamGroup(1, "Red", Color.red), // 红队
                    [2] = new TeamGroup(2, "Blue", Color.blue) // 蓝队
                };
            }

            // if (DataBase == null) DataBase = new PokemonDataBase();
        }

        [Server]
        public bool IsOnline(string userId)
        {
            return _onlineId2Conn.ContainsKey(userId);
        }


        [Server]
        public void UserLogin(string userId, NetworkConnectionToClient conn)
        {
            _onlineId2Conn.Add(userId, conn);
            _onLineConn2Id.Add(conn, userId);
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
        public void UserOffLine(string userId)
        {
            NetworkServer.DestroyPlayerForConnection(_onlineId2Conn[userId]);
            _onLineConn2Id.Remove(_onlineId2Conn[userId]);
            _onlineId2Conn.Remove(userId);
        }

        [Server]
        public void UserOffLine(NetworkConnectionToClient conn)
        {
            NetworkServer.DestroyPlayerForConnection(conn);
            _onlineId2Conn.Remove(_onLineConn2Id[conn]);
            _onLineConn2Id.Remove(conn);
        }

        [Server]
        public void Register(string userId, string playerName)
        {
            if (records.ContainsKey(userId)) return;

            // TODO 多样初始化
            records[userId] = new PlayerRecord
            {
                userId = userId,
                data = new PlayerData(userId, playerName, new PokemonData(PokemonEnum.玩家)),
                package = new PackageData(),
                position = Vector3.zero,
                teamGroup = TeamGroup.Default
            };
        }

        [Server]
        public bool Registered(string userId)
        {
            return records.ContainsKey(userId);
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

        [Server]
        public void QueryGroupData(int id, out TeamGroup teamGroup)
        {
            if (!id2TeamGroup.TryGetValue(id, out teamGroup))
            {
                teamGroup = TeamGroup.Default;
                Debug.LogError($"Group:{id} not found , use default group");
            }
        }
    }
}