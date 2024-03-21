using System;
using System.Collections.Generic;
using MemoryPack;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
        

        public void Init()
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

            // Debug.Log("ChatPanel Awake");
            GlobalManager.EventSystem.Register<ChatMessage>(OnNewMessage);
        }
        private void OnDestroy()
        {
            if (GlobalManager.EventSystem != null)
            {
                GlobalManager.EventSystem.UnRegister<ChatMessage>(OnNewMessage);
            }
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

        public bool IsOpen()
        {
            return gameObject.activeSelf;
        }

        public void Open()
        {
            Player.Local.DisableInput();
            _msgInput.ActivateInputField(); // Focus // EventSystem.current.SetSelectedGameObject(_msgInput.gameObject);
            gameObject.SetActive(true);
            RefreshChatList();
        }


        public void Close()
        {
            if (Player.Local != null)
            {
                Player.Local.EnableInput();
            }
            gameObject.SetActive(false);
        }

        private void OnNewMessage(ChatMessage msg)
        {
            // Debug.Log("OnNewMessage");
            _messages.Add(msg);
            if (IsOpen())
            {
                RefreshChatList();
            }
        }

        private void RefreshChatList()
        {
            _chatList.totalCount = _messages.Count;
            _chatList.RefillCells();
        }

        private void ItemRenderer(Transform tar, int idx)
        {
            tar.GetComponent<ChatMessageItem>().Bind(idx, _messages[idx]);
        }

    }
}