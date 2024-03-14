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
                return;
            }
        }

        public void OnExit(PokemonController owner)
        {
            // Debug.Log("PokemonBeAttackState OnExit");
            canExit = false;
            over = false;
        }
    }
}