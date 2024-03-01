using UnityToolkit;

namespace Game
{
    public class PokemonIdleState : State<PokemonController>
    {
        public override void OnEnter(PokemonController owner)
        {
            owner.animator.SetBool(PokemonController.idle, true);
        }

        public override void OnExit(PokemonController owner)
        {
            owner.animator.SetBool(PokemonController.idle, false);
        }
    }
}