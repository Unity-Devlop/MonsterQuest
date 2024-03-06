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
    }
}