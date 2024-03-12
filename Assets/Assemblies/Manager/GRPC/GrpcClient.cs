using System;
using Grpc.Core;
using Proto;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class GrpcClient : MonoSingleton<GrpcClient>
    {
        protected override bool DontDestroyOnLoad() => true;

        private GameService.GameServiceClient _gameService;
        public static GameService.GameServiceClient GameService => Singleton._gameService;
        
        private GlobalService.GlobalServiceClient _globalService;
        public static GlobalService.GlobalServiceClient GlobalService => Singleton._globalService;
        public static bool ready => SingletonNullable != null;

        private Channel _rpcChannel;
        public string address = "127.0.0.1:22333";

        protected override void OnInit()
        {
            Application.wantsToQuit += OnWantToQuit;
            _rpcChannel = new Channel(address, ChannelCredentials.Insecure);
            _gameService = new GameService.GameServiceClient(_rpcChannel);
            _globalService = new GlobalService.GlobalServiceClient(_rpcChannel);
        }

        protected override void OnDispose()
        {
            try
            {
                if (_rpcChannel.State != ChannelState.Shutdown)
                {
                    _rpcChannel.ShutdownAsync().Wait();
                }
            }
            catch (RpcException e)
            {
                Debug.LogError(e);
            }
        }

        private bool OnWantToQuit()
        {
            Application.wantsToQuit -= OnWantToQuit;
            try
            {
                if (_rpcChannel.State != ChannelState.Shutdown)
                {
                    _rpcChannel.ShutdownAsync().Wait();
                }
            }
            catch (RpcException e)
            {
                Debug.LogError(e);
                return true;
            }

            return true;
        }
    }
}