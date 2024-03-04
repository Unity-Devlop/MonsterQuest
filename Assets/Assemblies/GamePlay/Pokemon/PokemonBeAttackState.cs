using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonBeAttackState : State<PokemonController>, ITempAnimState
    {
        public bool canExit { get; private set; }

        public override void OnEnter(PokemonController owner)
        {
            // Debug.Log("PokemonBeAttackState OnEnter");
            canExit = false;
            owner.animator.SetTrigger(PokemonController.BeAttack);
        }

        public override void OnUpdate(PokemonController owner)
        {
            AnimatorStateInfo stateInfo = owner.animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Be_Attacked"))
            {
                return;
            }

            if (stateInfo.normalizedTime > 0.95f)
            {
                canExit = true;
                return;
            }
        }

        public override void OnExit(PokemonController owner)
        {
            // Debug.Log("PokemonBeAttackState OnExit");
            canExit = true;
        }
    }
}