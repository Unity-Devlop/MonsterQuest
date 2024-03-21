using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityToolkit;

namespace Game.UI
{
    public class GamePanel : UIPanel
    {
        private ChatPanel _chatPanel;
        private HUDPanel _hudPanel;

        private void Awake()
        {
            _chatPanel = transform.Find("ChatPanel").GetComponent<ChatPanel>();
            _hudPanel = transform.Find("HUDPanel").GetComponent<HUDPanel>();
            
            _chatPanel.Init();
            _hudPanel.Init();
        }

        public override void OnOpened()
        {
            Player.OnLocalPlayerSpawned += OnLocalPlayerSpawned;
            base.OnOpened();
        }

        public override void OnClosed()
        {
            Player.OnLocalPlayerSpawned -= OnLocalPlayerSpawned;
            _chatPanel.Close();
            _hudPanel.Close();
            base.OnClosed();
        }

        private void OnLocalPlayerSpawned()
        {
            _hudPanel.Open();
        }

        public void CloseSub()
        {
            _chatPanel.Close();
        }

        public void OpenChatPanel()
        {
            _chatPanel.Open();
        }
    }
}