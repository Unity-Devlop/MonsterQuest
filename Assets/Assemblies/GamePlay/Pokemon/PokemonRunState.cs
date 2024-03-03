using UnityToolkit;

namespace Game
{
    public class PokemonRunState : State<PokemonController>
    {
        public override void OnEnter(PokemonController owner)
        {
            owner.animator.SetBool(PokemonController.run, true);
        }

        public override void OnExit(PokemonController owner)
        {
            owner.animator.SetBool(PokemonController.run, false);
        }
    }
}