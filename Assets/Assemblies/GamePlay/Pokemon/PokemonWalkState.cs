using UnityToolkit;

namespace Game
{
    public class PokemonWalkState : State<PokemonController>
    {
        public override void OnEnter(PokemonController owner)
        {
            // if (owner.isClient)
            // {
            //     owner.CmdSetAnimBool(PokemonController.walk, true);
            // }
            // else if (owner.isServerOnly)
            // {
                owner.animator.SetBool(PokemonController.walk, true);
            // }
        }

        public override void OnExit(PokemonController owner)
        {
            // if (owner.isClient)
            // {
            //     owner.CmdSetAnimBool(PokemonController.walk, false);
            // }
            // else if (owner.isServerOnly)
            // {
                owner.animator.SetBool(PokemonController.walk, false);
            // }
        }
    }
}