using UnityToolkit;

namespace Game
{
    public class InputManager: MonoSingleton<InputManager>
    {
        protected override bool DontDestroyOnLoad() => true;

        public GameInput input { get; private set; }
        protected override void OnInit()
        {
            input = new GameInput();
            input.Enable();
        }

        protected override void OnDispose()
        {
            input.Disable();
            input = null;
        }
    }
}