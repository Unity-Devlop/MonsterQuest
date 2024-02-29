using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class GrpcManager : MonoSingleton<GrpcManager>
    {
        protected override bool DontDestroyOnLoad() => true;

        protected override void OnDispose()
        {
            base.OnDispose();
        }

        protected override void OnInit()
        {
            Application.wantsToQuit += OnWantToQuit;
            base.OnInit();
        }

        private bool OnWantToQuit()
        {
            Application.wantsToQuit -= OnWantToQuit;
            return true;
        }
    }
}