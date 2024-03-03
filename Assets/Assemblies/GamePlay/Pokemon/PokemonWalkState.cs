using UnityToolkit;

namespace Game
{
    public class PokemonWalkState : State<PokemonController>
    {
        public override void OnEnter(PokemonController owner)
        {
            owner.animator.SetBool(PokemonController.walk, true);
        }

        public override void OnExit(PokemonController owner)
        {
            owner.animator.SetBool(PokemonController.walk, false);
        }
    }
}