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

        public override void OnStopServer()
        {
            DestroyImmediate(PokemonServer.Singleton);
            NetworkServer.UnregisterHandler<RequestEnterGameMessage>();
        }

        public override void OnServerConnect(NetworkConnectionToClient conn)
        {
            
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            PokemonServer.Singleton.OffLine(conn);
        }

        private void RequestEnterGame(NetworkConnectionToClient connection, RequestEnterGameMessage msg)
        {
            if (!PokemonServer.Singleton.Registered(msg.userId))
            {
                Debug.Log($"Id:[{msg.userId}],Name:[{msg.userName}]第一次上线,初始化角色信息\n");
                PokemonServer.Singleton.Register(msg.userId, msg.userName);
            }

            if (PokemonServer.Singleton.IsOnline(msg.userId))
            {
                Debug.Log($"Id:[{msg.userId}],Name:[{msg.userName}]已经在线,踢出之前的玩家\n");
                PokemonServer.Singleton.OffLine(msg.userId);
            }
            
            PokemonServer.Singleton.OnLine(msg.userId, connection);
            
            // Query Last Pos
            PokemonServer.Singleton.QueryPosition(msg.userId, out Vector3 position);
            
            // 在网络上创建角色
            GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
            player.name = $"player:[{msg.userName}]-[{msg.userName}]";
            NetworkServer.AddPlayerForConnection(connection, player);
        }

        #endregion

        #region Client

        public override void OnStartClient()
        {
        }

        public override void OnStopClient()
        {
            
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            NetworkClient.Send(new RequestEnterGameMessage
            {
                userId = Authentication.userId,
                userName = Authentication.userName
            });
        }

        #endregion
    }
}