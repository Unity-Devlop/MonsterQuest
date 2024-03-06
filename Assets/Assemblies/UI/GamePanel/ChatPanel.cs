using System;
using UnityEngine;
using UnityToolkit;

namespace Game.UI
{
    public class ChatPanel : MonoBehaviour, IUISubPanel
    {
        private LoopVerticalScrollRect _chatList;
        private EasyGameObjectPool _itemPool;

        private void Awake()
        {
            Transform list = transform.Find("List");
            _chatList = list.GetComponent<LoopVerticalScrollRect>();
            _itemPool = list.GetComponent<EasyGameObjectPool>();
            _chatList.ItemProvider = _ => _itemPool.Get();
            _chatList.itemRenderer = ItemRenderer;
            _chatList.ItemReturn = go => { _itemPool.Release(go.gameObject); };
            // Open();
        }

        public void Open()
        {
            gameObject.SetActive(true);
            PokemonClient.Singleton.OnNewMessage += OnNewMessage;
        }


        public void Close()
        {
            gameObject.SetActive(false);
            if(PokemonClient.SingletonNullable==null) return;
            PokemonClient.Singleton.OnNewMessage -= OnNewMessage;
        }

        private void OnNewMessage()
        {
            _chatList.totalCount = PokemonClient.Singleton.ChatMessages.Count;
            _chatList.RefillCells();
        }

        private void ItemRenderer(Transform tar, int idx)
        {
            tar.GetComponent<ChatMessageItem>().Bind(idx, PokemonClient.Singleton.ChatMessages[idx]);
        }
    }
}