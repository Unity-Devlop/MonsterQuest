using System;
using System.Collections.Generic;
using MemoryPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityToolkit;

namespace Game.UI
{
    public class ChatPanel : MonoBehaviour, IUISubPanel
    {
        private LoopVerticalScrollRect _chatList;
        private EasyGameObjectPool _itemPool;
        private Button _sendButton;
        private TMP_InputField _msgInput;
        private List<ChatMessage> _messages; // TODO Move To Other Place
        private void Awake()
        {
            Transform list = transform.Find("List");
            _chatList = list.GetComponent<LoopVerticalScrollRect>();
            _itemPool = list.GetComponent<EasyGameObjectPool>();
            _chatList.ItemProvider = _ => _itemPool.Get();
            _chatList.itemRenderer = ItemRenderer;
            _chatList.ItemReturn = go => { _itemPool.Release(go.gameObject); };
            // Open();
            _msgInput = transform.Find("MsgInput").GetComponent<TMP_InputField>();
            _sendButton = transform.Find("SendButton").GetComponent<Button>();
            _sendButton.onClick.AddListener(OnSendButtonClicked);
            
            _messages = new List<ChatMessage>();
        }

        private void OnSendButtonClicked()
        {
            ChatMessage msg = new ChatMessage()
            {
                uid = Authentication.userId,
                name = Authentication.playerName,
                content = _msgInput.text
            };
            Player.Local.CmdSendChatMessage(MemoryPackSerializer.Serialize(msg));
            _msgInput.text = "";
        }

        public void Open()
        {
            gameObject.SetActive(true);
            // GlobalManager.EventSystem.Register(OnNewMessage);
            // Debug.Log("ChatPanel Open");
            GlobalManager.EventSystem.Register<ChatMessage>(OnNewMessage);
        }


        public void Close()
        {
            gameObject.SetActive(false);
            if (GlobalManager.EventSystem != null)
            {
                GlobalManager.EventSystem.UnRegister<ChatMessage>(OnNewMessage);
            }
        }

        private void OnNewMessage(ChatMessage msg)
        {
            // Debug.Log("OnNewMessage");
            _messages.Add(msg);
            _chatList.totalCount = _messages.Count;
            _chatList.RefillCells();
        }

        private void ItemRenderer(Transform tar, int idx)
        {
            tar.GetComponent<ChatMessageItem>().Bind(idx,_messages[idx]);
        }
    }
}