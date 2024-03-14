using UnityToolkit;

namespace Game
{
    public class PlayerAttackState : IState<PlayerController>
    {
        public void OnEnter(PlayerController owner)
        {
            owner.animator.SetBool(IEntity.attack, true);
        }

        public void OnUpdate(PlayerController owner)
        {
            
        }

        public void OnExit(PlayerController owner)
        {
            owner.animator.SetBool(IEntity.attack, false);
        }
    }
}