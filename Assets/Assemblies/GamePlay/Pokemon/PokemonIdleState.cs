using UnityToolkit;

namespace Game
{
    public class PokemonIdleState : State<PokemonController>
    {
        public override void OnEnter(PokemonController owner)
        {
            // if (owner.isClient)
            // {
            //     owner.CmdSetAnimBool(PokemonController.idle, true);
            // }
            // else if (owner.isServerOnly)
            // {
                owner.animator.SetBool(PokemonController.idle, true);
            // }
        }

        public override void OnExit(PokemonController owner)
        {
            // if (owner.isClient)
            // {
            //     owner.CmdSetAnimBool(PokemonController.idle, false);
            // }
            // else if (owner.isServerOnly)
            // {
                owner.animator.SetBool(PokemonController.idle, false);
            // }
        }
    }
}