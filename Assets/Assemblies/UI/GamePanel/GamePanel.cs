using System;
using UnityToolkit;

namespace Game.UI
{
    public class GamePanel : UIPanel
    {
        private ChatPanel _chatPanel;

        private void Awake()
        {
            _chatPanel = transform.Find("ChatPanel").GetComponent<ChatPanel>();
        }

        public override void OnOpened()
        {
            base.OnOpened();
            PlayerController.OnLocalPlayerSpawned += OnLocalPlayerSpawned;
        }

        public override void OnClosed()
        {
            base.OnClosed();
            PlayerController.OnLocalPlayerSpawned -= OnLocalPlayerSpawned;
            _chatPanel.Close();
        }

        private void OnLocalPlayerSpawned()
        {
            _chatPanel.Open();
        }
    }
}