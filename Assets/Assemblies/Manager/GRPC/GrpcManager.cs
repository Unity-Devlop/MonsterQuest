using System;
using Grpc.Core;
using Proto;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class GrpcManager : MonoSingleton<GrpcManager>
    {
        protected override bool DontDestroyOnLoad() => true;
        public GameService.GameServiceClient Client { get; private set; }
        private Channel _rpcChannel;
        public string address = "127.0.0.1:22333";

        protected override void OnInit()
        {
            Application.wantsToQuit += OnWantToQuit;
            _rpcChannel = new Channel(address, ChannelCredentials.Insecure);
            Client = new GameService.GameServiceClient(_rpcChannel);
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