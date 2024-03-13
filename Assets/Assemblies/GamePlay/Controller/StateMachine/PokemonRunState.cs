using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonRunState :MonoBehaviour, IState<PokemonController>
    {
        public  void OnEnter(PokemonController owner)
        {
            owner.animator.SetBool(PokemonStateMachine.run, true);
        }

        public void OnUpdate(PokemonController owner)
        {
            
        }

        public  void OnExit(PokemonController owner)
        {
            owner.animator.SetBool(PokemonStateMachine.run, false);
        }
    }
}