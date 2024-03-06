using System;
using System.Collections.Generic;
using Proto;
using UnityToolkit;

namespace Game
{
    public sealed class PokemonClient : MonoSingleton<PokemonClient>
    {
        protected override bool DontDestroyOnLoad() => false;
        
        public List<ChatMessage> ChatMessages { get; private set; }
        public event Action OnNewMessage; 

        protected override void OnInit()
        {
            ChatMessages = new List<ChatMessage>();
            // GrpcManager.Singleton.Client.StartPublicChat(new ChatRequest()
            // {
            //     Uid = Authentication.userId,
            // });
        }

        protected override void OnDispose()
        {
            ChatMessages.Clear();
            ChatMessages = null;
        }
        
        // [Sirenix.OdinInspector.Button]
        // public void DebugMessage(string msg)
        // {
        //     ChatMessages.Add(new ChatMessage
        //     {
        //         SenderUid = 0,
        //         Content = msg,
        //     });
        //     OnNewMessage?.Invoke();
        // }
    }
}