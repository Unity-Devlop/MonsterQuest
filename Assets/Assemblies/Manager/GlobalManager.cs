using System;
using Cysharp.Threading.Tasks;
using Game.UI;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolkit;

namespace Game
{
    public class GlobalManager : MonoSingleton<GlobalManager>
    {
        protected override bool DontDestroyOnLoad() => true;
        [field: SerializeField] public Camera MainCamera { get; private set; }
        public LayerMask hittableLayer;

        public ConfigTable configTable;

        public static TypeEventSystem EventSystem { get; private set; }

        protected override void OnInit()
        {
            EventSystem = new TypeEventSystem();
        }

        protected override void OnDispose()
        {
            EventSystem = null;
        }

        private void Start()
        {
// #if UNITY_EDITOR
            if (SceneManager.GetActiveScene().name == "Dev") // TODO 快速开发用
            {
                return;
            }
// #endif
            ToHome();
        }

        public async void ToHome()
        {
            await SceneManager.LoadSceneAsync("Home");
            UIRoot.Singleton.CloseAll();
            UIRoot.Singleton.OpenPanel<HomePanel>();
        }

        public void ConnectToServer(Uri uri)
        {
            NetworkManager.singleton.StartClient(uri);
        }

        public void HostGame()
        {
            NetworkManager.singleton.StartHost();
        }
    }
}