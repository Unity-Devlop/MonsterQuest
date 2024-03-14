using UnityToolkit;

namespace Game
{
    public class PlayerRunState : IState<PlayerController>
    {
        public void OnEnter(PlayerController owner)
        {
            owner.animator.SetBool(IEntity.run, true);
        }

        public void OnUpdate(PlayerController owner)
        {
           
        }

        public void OnExit(PlayerController owner)
        {
            owner.animator.SetBool(IEntity.run, false);
        }
    }
}