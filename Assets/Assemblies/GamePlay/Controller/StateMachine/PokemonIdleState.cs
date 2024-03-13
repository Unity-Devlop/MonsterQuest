using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonIdleState : MonoBehaviour,IState<PokemonController>
    {
        public  void OnEnter(PokemonController owner)
        {
            // if (owner.isClient)
            // {
            //     owner.CmdSetAnimBool(PokemonStateMachine.idle, true);
            // }
            // else if (owner.isServerOnly)
            // {
                owner.animator.SetBool(PokemonStateMachine.idle, true);
            // }
        }

        public void OnUpdate(PokemonController owner)
        {
            
        }

        public  void OnExit(PokemonController owner)
        {
            // if (owner.isClient)
            // {
            //     owner.CmdSetAnimBool(PokemonStateMachine.idle, false);
            // }
            // else if (owner.isServerOnly)
            // {
                owner.animator.SetBool(PokemonStateMachine.idle, false);
            // }
        }
    }
}