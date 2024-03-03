using Mirror;
using UnityEngine;

namespace Game
{
    public class PokemonNetworkManager : NetworkManager
    {
        #region Server

        public override void OnStartServer()
        {
            gameObject.AddComponent<PokemonServer>();
            NetworkServer.RegisterHandler<RequestEnterGameMessage>(RequestEnterGame);
        }

        
        
        public override void OnDestroy()
        {
            base.OnApplicationQuit();
        }

        public override void OnApplicationQuit()
        {
            base.OnApplicationQuit();
        }

        public override void OnStopServer()
        {
            if (PokemonServer.SingletonNullable != null)
            {
                DestroyImmediate(PokemonServer.SingletonNullable);
            }
            NetworkServer.UnregisterHandler<RequestEnterGameMessage>();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            base.OnServerConnect(conn);
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            if (PokemonServer.SingletonNullable != null)
            {
                PokemonServer.SingletonNullable.OffLine(conn);
            }
        }

        private void RequestEnterGame(NetworkConnectionToClient connection, RequestEnterGameMessage msg)
        {
            if (!PokemonServer.Singleton.Registered(msg.userId))
            {
                Debug.Log($"Id:[{msg.userId}],Name:[{msg.playerName}]第一次上线,初始化角色信息\n");
                PokemonServer.Singleton.Register(msg.userId, msg.playerName);
            }

            if (PokemonServer.Singleton.IsOnline(msg.userId))
            {
                Debug.Log($"Id:[{msg.userId}],Name:[{msg.playerName}]已经在线,不可加入\n");
                connection.Disconnect();
                return;
            }

            PokemonServer.Singleton.OnLine(msg.userId, connection);

            // Query Last Pos
            PokemonServer.Singleton.QueryPosition(msg.userId, out Vector3 position);

            // 在网络上创建角色
            GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.name = $"player:[{msg.playerName}]";
            PlayerController component = player.GetComponent<PlayerController>();
            PokemonServer.Singleton.QueryPlayerData(msg.userId, out var playerData);
            PokemonServer.Singleton.QueryPackageData(msg.userId, out var packageData);
            component.ServerInitData(playerData, packageData, connection, position);
            NetworkServer.AddPlayerForConnection(connection, player);
        }

        #endregion

        #region Client

        public override void OnStartClient()
        {
            base.OnStartClient();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            NetworkClient.Send(new RequestEnterGameMessage
            {
                userId = Authentication.userId,
                playerName = Authentication.playerName
            });
        }

        #endregion
    }
}