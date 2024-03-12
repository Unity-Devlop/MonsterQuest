using System;
using UnityEngine;
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
        }

        public override void OnOpened()
        {
            PlayerController.OnLocalPlayerSpawned += OnLocalPlayerSpawned;
            base.OnOpened();
        }

        public override void OnClosed()
        {
            PlayerController.OnLocalPlayerSpawned -= OnLocalPlayerSpawned;
            _chatPanel.Close();
            _hudPanel.Close();
            base.OnClosed();
        }

        private void OnLocalPlayerSpawned()
        {
            // Debug.Log("OnLocalPlayerSpawned");
            _chatPanel.Open();
            _hudPanel.Open();
        }
    }
}