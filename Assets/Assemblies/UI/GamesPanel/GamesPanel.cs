using System;
using UnityEngine.UI;
using UnityToolkit;

namespace Game.UI
{
    public class GamesPanel : UIPanel
    {
        private Button _backButton;
        private Button _joinButton;
        private GameSelectPanel _gameSelectPanel;

        private void Awake()
        {
            _gameSelectPanel = transform.Find("GameSelectPanel").GetComponent<GameSelectPanel>();
            
            _backButton = transform.Find("BackButton").GetComponent<Button>();
            _joinButton = transform.Find("JoinButton").GetComponent<Button>();
            
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _joinButton.onClick.AddListener(OnJoinButtonClicked);
            
        }

        public override void OnOpened()
        {
            base.OnOpened();
            _gameSelectPanel.Open();
        }
        
        public override void OnClosed()
        {
            base.OnClosed();
            _gameSelectPanel.Close();
        }

        private void OnJoinButtonClicked()
        {
            CloseSelf();
            // GlobalManager.Singleton.ConnectToServer(uri);
        }

        private void OnBackButtonClicked()
        {
            CloseSelf();
            UIRoot.Singleton.OpenPanel<HomePanel>();
        }
    }
}