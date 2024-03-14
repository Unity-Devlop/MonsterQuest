using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PlayerBeAttackState : IState<PlayerController>, ITempAnimState
    {
        public bool canExit => over;
        public bool over { get; private set; }

        public void OnEnter(PlayerController owner)
        {
            owner.canBeHit = false;
            owner.animator.SetTrigger(IEntity.BeAttack);
        }

        public void OnUpdate(PlayerController owner)
        {
            AnimatorStateInfo stateInfo = owner.animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Be_Attacked"))
            {
                return;
            }

            if (stateInfo.normalizedTime > 0.95f)
            {
                over = true;
                return;
            }
        }

        public void OnExit(PlayerController owner)
        {
            owner.canBeHit = true;
        }
    }
}