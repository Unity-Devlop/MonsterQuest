using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    [Serializable]
    public class PokemonServer : MonoSingleton<PokemonServer>
    {
        protected override bool DontDestroyOnLoad() => false;

        private static readonly string UserSetPath = "UserSet.json";
        private static readonly string PlayerDataPath = "PlayerData.json";
        private static readonly string PackageDataPath = "PackageData.json";
        private static readonly string PositionPath = "Position.json";
        private static readonly string NameSetPath = "NameSet.json";
        // private static readonly string DataBasePath = "DataBase.json";
        private static readonly string TeamGroupPath = "TeamGroup.json";

        // 玩家基础数据
        [SerializeField] private SerializableDictionary<string, PlayerData> id2PlayerData;

        // 玩家背包数据
        [SerializeField] private SerializableDictionary<string, PackageData> id2PackageData;

        // 玩家下线位置
        [SerializeField] private SerializableDictionary<string, Vector3> id2Position;

        [SerializeField] private SerializableDictionary<int, TeamGroup> id2TeamGroup;


        // 用户名集合
        [Sirenix.OdinInspector.ShowInInspector]
        private HashSet<string> _nameSet;

        // [field: SerializeField] public PokemonDataBase DataBase { get; private set; }


        // 非持久化数据
        private Dictionary<string, NetworkConnectionToClient> _onlineId2Conn;

        private Dictionary<NetworkConnectionToClient, string> _onLineConn2Id;

        private Dictionary<string, PlayerController> _id2PlayerController;

        // 注册过的用户
        [Sirenix.OdinInspector.ShowInInspector]
        private HashSet<string> _userSet;


        protected override void OnInit()
        {
            _onlineId2Conn = new Dictionary<string, NetworkConnectionToClient>();
            _onLineConn2Id = new Dictionary<NetworkConnectionToClient, string>();
            _id2PlayerController = new Dictionary<string, PlayerController>();
            Load();
        }

        protected override void OnDispose()
        {
            Save();
        }

        public void AddPlayer(PlayerController controller)
        {
            _id2PlayerController.Add(controller.userId, controller);
        }

        public void RemovePlayer(PlayerController controller)
        {
            _id2PlayerController.Remove(controller.userId);
        }

        [Server]
        public IEnumerator<PlayerController> OnlinePlayers()
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
            JsonUtil.SaveJsonToStreamingAssets(PlayerDataPath, id2PlayerData);
            JsonUtil.SaveJsonToStreamingAssets(PackageDataPath, id2PackageData);
            JsonUtil.SaveJsonToStreamingAssets(PositionPath, id2Position);
            JsonUtil.SaveJsonToStreamingAssets(UserSetPath, _userSet);
            JsonUtil.SaveJsonToStreamingAssets(NameSetPath, _nameSet);
            JsonUtil.SaveJsonToStreamingAssets(TeamGroupPath, id2TeamGroup);
            // JsonUtil.SaveJsonToStreamingAssets(DataBasePath, DataBase);
        }

        [Server]
        [Sirenix.OdinInspector.Button]
        private void Load()
        {
            id2PlayerData =
                JsonUtil.LoadJsonFromStreamingAssets<SerializableDictionary<string, PlayerData>>(PlayerDataPath);
            id2PackageData =
                JsonUtil.LoadJsonFromStreamingAssets<SerializableDictionary<string, PackageData>>(PackageDataPath);
            id2Position =
                JsonUtil.LoadJsonFromStreamingAssets<SerializableDictionary<string, Vector3>>(PositionPath);
            _userSet =
                JsonUtil.LoadJsonFromStreamingAssets<HashSet<string>>(UserSetPath);
            _nameSet =
                JsonUtil.LoadJsonFromStreamingAssets<HashSet<string>>(NameSetPath);

            id2TeamGroup = JsonUtil.LoadJsonFromStreamingAssets<SerializableDictionary<int, TeamGroup>>(TeamGroupPath);
            // DataBase = JsonUtil.LoadJsonFromStreamingAssets<PokemonDataBase>(DataBasePath);


            if (id2PlayerData == null) id2PlayerData = new SerializableDictionary<string, PlayerData>();
            if (id2PackageData == null) id2PackageData = new SerializableDictionary<string, PackageData>();
            if (id2Position == null) id2Position = new SerializableDictionary<string, Vector3>();
            if (_userSet == null) _userSet = new HashSet<string>();
            if (_nameSet == null) _nameSet = new HashSet<string>();
            if (id2TeamGroup == null)
            {
                id2TeamGroup = new SerializableDictionary<int, TeamGroup>();
                // preset groups
                id2TeamGroup[0] = TeamGroup.Default;
                id2TeamGroup[1] = new TeamGroup(1, "Red", Color.red); // 红队
                id2TeamGroup[2] = new TeamGroup(2, "Blue", Color.blue); // 蓝队
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
            if (_userSet.Contains(userId)) return;
            // TODO 多样初始化
            _userSet.Add(userId);
            _nameSet.Add(playerName);
            id2PlayerData[userId] = new PlayerData(userId, playerName, new PokemonData(PokemonEnum.妙蛙种子));
            id2PackageData[userId] = new PackageData();
            id2Position[userId] = Vector3.zero;
        }

        [Server]
        public bool Registered(string userId)
        {
            return _userSet.Contains(userId);
        }

        [Server]
        public bool QueryPlayerData(string userId, out PlayerData data)
        {
            return id2PlayerData.TryGetValue(userId, out data);
        }

        [Server]
        public bool QueryPackageData(string userId, out PackageData data)
        {
            return id2PackageData.TryGetValue(userId, out data);
        }

        [Server]
        public bool QueryPosition(string userId, out Vector3 position)
        {
            if (_id2PlayerController.TryGetValue(userId, out var controller) && controller.state == PlayerState.Ready)
            {
                position = controller.pokemonController.pokemonPosition;
                return true;
            }

            return id2Position.TryGetValue(userId, out position);
        }

        [Server]
        public void UpdatePosition(string userId, Vector3 pokemonPosition)
        {
            id2Position[userId] = pokemonPosition;
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