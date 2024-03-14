using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonRunState : IState<PokemonController>
    {
        public  void OnEnter(PokemonController owner)
        {
            owner.animator.SetBool(IEntity.run, true);
        }

        public void OnUpdate(PokemonController owner)
        {
            
        }

        public  void OnExit(PokemonController owner)
        {
            owner.animator.SetBool(IEntity.run, false);
        }
    }
}