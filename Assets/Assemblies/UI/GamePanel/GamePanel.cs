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
        private TeamPanel _teamPanel;

        private void Awake()
        {
            _chatPanel = transform.Find("ChatPanel").GetComponent<ChatPanel>();
            _hudPanel = transform.Find("HUDPanel").GetComponent<HUDPanel>();
            _teamPanel = transform.Find("TeamPanel").GetComponent<TeamPanel>();
            
            _chatPanel.Init();
            _hudPanel.Init();
            _teamPanel.Init();
        }

        public override void OnOpened()
        {
            Player.OnLocalPlayerSpawned += OnLocalPlayerSpawned;
            base.OnOpened();
        }

        public override void OnClosed()
        {
            Player.OnLocalPlayerSpawned -= OnLocalPlayerSpawned;
            _teamPanel.Close();
            _chatPanel.Close();
            _hudPanel.Close();
            base.OnClosed();
        }

        private void OnLocalPlayerSpawned()
        {
            _hudPanel.Open();
            _teamPanel.Open();
        }
        

        public void OpenChatPanel()
        {
            _chatPanel.Open();
        }
    }
}