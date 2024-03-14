using UnityToolkit;

namespace Game
{
    public class PlayerBeAttackState : IState<PlayerController> //,ITempAnimState
    {
        public void OnEnter(PlayerController owner)
        {
            owner.animator.SetTrigger(IEntity.BeAttack);
        }

        public void OnUpdate(PlayerController owner)
        {
        }

        public void OnExit(PlayerController owner)
        {
        }
    }
}