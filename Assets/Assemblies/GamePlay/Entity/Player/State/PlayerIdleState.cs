using UnityToolkit;

namespace Game
{
    public class PlayerIdleState : IState<PlayerController>
    {
        public void OnEnter(PlayerController owner)
        {
            owner.animator.SetBool(IEntity.idle, true);
        }

        public void OnUpdate(PlayerController owner)
        {
            
        }

        public void OnExit(PlayerController owner)
        {
            owner.animator.SetBool(IEntity.idle, false);
        }
    }
}