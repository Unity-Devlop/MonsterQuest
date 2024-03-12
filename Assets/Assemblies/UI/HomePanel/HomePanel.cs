using System;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityToolkit;

namespace Game.UI
{
    public class HomePanel : UIPanel
    {
        private Button _browseGamesButton;
        private Button _hostGameButton;
        private Button _exitGameButton;

        private Button _debugStartClientButton;
        private Button _debugStartServerButton;
        private Button _debugStartHostButton;


        private void Awake()
        {
            _browseGamesButton = transform.Find("StartPanel/BrowseGamesButton").GetComponent<Button>();
            _hostGameButton = transform.Find("StartPanel/HostGameButton").GetComponent<Button>();
            _exitGameButton = transform.Find("StartPanel/ExitGameButton").GetComponent<Button>();

            _browseGamesButton.onClick.AddListener(OnBrowseGamesButtonClicked);
            _hostGameButton.onClick.AddListener(OnHostGameButtonClicked);
            _exitGameButton.onClick.AddListener(OnExitGameButtonClicked);

            _debugStartClientButton = transform.Find("DebugPanel/StartClientButton").GetComponent<Button>();
            _debugStartServerButton = transform.Find("DebugPanel/StartServerButton").GetComponent<Button>();
            _debugStartHostButton = transform.Find("DebugPanel/StartHostButton").GetComponent<Button>();

            _debugStartClientButton.onClick.AddListener(OnDebugStartClientButtonClicked);
            _debugStartServerButton.onClick.AddListener(OnDebugStartServerButtonClicked);
            _debugStartHostButton.onClick.AddListener(OnDebugStartHostButtonClicked);
        }

        private async void OnDebugStartServerButtonClicked()
        {
            UIRoot.Singleton.CloseAll();
            await SceneManager.LoadSceneAsync("Game");
            NetworkManager.singleton.StartServer();
        }

        private async void OnDebugStartHostButtonClicked()
        {
            UIRoot.Singleton.CloseAll();
            await SceneManager.LoadSceneAsync("Game");
            NetworkManager.singleton.StartHost();
        }

        private async void OnDebugStartClientButtonClicked()
        {
            UIRoot.Singleton.CloseAll();
            await SceneManager.LoadSceneAsync("Game");
            NetworkManager.singleton.StartClient();
        }

        private void OnHostGameButtonClicked()
        {
            CloseSelf();
            NetworkManager.singleton.StartHost(); // TODO 主动创建
            // UIRoot.Singleton.OpenPanel<HostGamePanel>();
        }

        private void OnBrowseGamesButtonClicked()
        {
            CloseSelf();
            UIRoot.Singleton.OpenPanel<GamesPanel>();
        }

        private void OnExitGameButtonClicked()
        {
            Application.Quit();
        }
    }
}