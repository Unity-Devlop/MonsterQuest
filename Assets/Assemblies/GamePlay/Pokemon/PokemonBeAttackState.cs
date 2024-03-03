using UnityToolkit;

namespace Game
{
    public class PokemonBeAttackState : State<PokemonController>
    {
        public override void OnEnter(PokemonController owner)
        {
            owner.animator.SetBool(PokemonController.BeAttack, true);
        }
        
        // public override void OnUpdate(PokemonController owner)
        // {
        //     if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
        //     {
        //         owner.animator.SetBool(PokemonController.beAttack, false);
        //         owner.HandleIdle();
        //         return;
        //     }
        // }

        public override void OnExit(PokemonController owner)
        {
            owner.animator.SetBool(PokemonController.BeAttack, false);
        }
    }
}