using Cysharp.Threading.Tasks.Triggers;
using Game.UI;
using Mirror;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonNetworkManager : NetworkManager
    {
        #region Server

        public override void OnStartServer()
        {
            new GameObject("PokemonServer").AddComponent<PokemonServer>();
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
                DestroyImmediate(PokemonServer.Singleton.gameObject);
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
                PokemonServer.SingletonNullable.UserOffLine(conn);
            }
        }

        private async void RequestEnterGame(NetworkConnectionToClient connection, RequestEnterGameMessage msg)
        {
            if (!await PokemonServer.Singleton.Registered(msg.userId))
            {
                Debug.Log($"Id:[{msg.userId}],Name:[{msg.playerName}]第一次上线,初始化角色信息\n");
                await PokemonServer.Singleton.Register(msg.userId, msg.playerName);
            }

            if (PokemonServer.Singleton.IsOnline(msg.userId))
            {
                Debug.Log($"Id:[{msg.userId}],Name:[{msg.playerName}]已经在线,不可加入\n");
                connection.Disconnect();
                return;
            }

            PokemonServer.Singleton.UserLogin(msg.userId, connection);

            // Query Last Pos
            PokemonServer.Singleton.QueryPosition(msg.userId, out Vector3 position);

            // 在网络上创建角色
            GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.name = $"player:[{msg.playerName}]";
            Player component = player.GetComponent<Player>();
            PokemonServer.Singleton.QueryPlayerData(msg.userId, out var playerData);
            PokemonServer.Singleton.QueryPackageData(msg.userId, out var packageData);
            component.ServerInitData(playerData, packageData, connection, position);
            NetworkServer.AddPlayerForConnection(connection, player);
        }

        #endregion

        #region Client

        public override void OnStartClient()
        {
        }

        public override void OnStopClient()
        {
            NetworkClient.UnregisterHandler<RequestEnterGameMessage>();
            UIRoot.Singleton.CloseAll(); // TODO: Close all panels
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            NetworkClient.Send(new RequestEnterGameMessage
            {
                userId = Authentication.userId,
                playerName = Authentication.playerName
            });
            UIRoot.Singleton.OpenPanel<GamePanel>(); // TODO: Open game panel
        }

        #endregion
    }
}