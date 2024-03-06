using System;
using System.Collections.Generic;
using Proto;
using UnityToolkit;

namespace Game
{
    public sealed class PokemonClient : MonoSingleton<PokemonClient>
    {
        protected override bool DontDestroyOnLoad() => false;
        
        public List<ChatMessage> messages { get; private set; }
        public event Action OnNewMessage; 

        protected override void OnInit()
        {
            messages = new List<ChatMessage>();
        }

        protected override void OnDispose()
        {
        }
        
        [Sirenix.OdinInspector.Button]
        public void DebugMessage(string msg)
        {
            messages.Add(new ChatMessage
            {
                SenderUid = 0,
                Content = msg,
            });
            OnNewMessage?.Invoke();
        }
    }
}