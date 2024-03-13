using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonWalkState : MonoBehaviour, IState<PokemonController>
    {
        public void OnEnter(PokemonController owner)
        {
            owner.animator.SetBool(PokemonStateMachine.walk, true);
        }

        public void OnUpdate(PokemonController owner)
        {
        }

        public void OnExit(PokemonController owner)
        {
            owner.animator.SetBool(PokemonStateMachine.walk, false);
        }
    }
}