using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonWalkState : IState<PokemonController>
    {
        public void OnEnter(PokemonController owner)
        {
            owner.animator.SetBool(IEntity.walk, true);
        }

        public void OnUpdate(PokemonController owner)
        {
        }

        public void OnExit(PokemonController owner)
        {
            owner.animator.SetBool(IEntity.walk, false);
        }
    }
}