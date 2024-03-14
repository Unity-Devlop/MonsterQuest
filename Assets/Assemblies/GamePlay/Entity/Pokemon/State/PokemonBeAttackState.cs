using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonBeAttackState : IState<PokemonController>, ITempAnimState
    {
        public bool canExit { get; private set; }
        public bool over { get; private set; }

        public void OnEnter(PokemonController owner)
        {
            // Debug.Log("PokemonBeAttackState OnEnter");
            owner.canBeHit = false;
            canExit = false;
            over = false;
            owner.animator.SetTrigger(IEntity.BeAttack);
        }

        public void OnUpdate(PokemonController owner)
        {
            AnimatorStateInfo stateInfo = owner.animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Be_Attacked"))
            {
                return;
            }

            if (stateInfo.normalizedTime > 0.95f)
            {
                canExit = true;
                over = true;
                owner.stateMachine.Change<PokemonIdleState>(owner);
                return;
            }
        }

        public void OnExit(PokemonController owner)
        {
            canExit = false;
            over = false;
            owner.canBeHit = true;
        }
    }
}