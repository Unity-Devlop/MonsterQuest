using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonStateMachine : StateMachine<PokemonController>
    {
        public PokemonStateMachine(PokemonController owner) : base(owner)
        {
        }
    }
}