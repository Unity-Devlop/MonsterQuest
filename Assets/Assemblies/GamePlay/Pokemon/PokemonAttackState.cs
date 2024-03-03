using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonAttackState : State<PokemonController>, ITempAnimState
    {
        private bool _complete;
        public bool canExit { get; private set; }

        public override void OnEnter(PokemonController owner)
        {
            owner.animator.SetBool(PokemonController.attack, true);
            _complete = false;
            canExit = false; // TODO 必须要打出关键帧后才能退出
        }

        public override void OnUpdate(PokemonController owner)
        {
            // TODO 必须要打出关键帧后才能退出
            // if (!canExit && owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.45f) ;
            // {
            //     canExit = true;
            // }
            // 动画放结束就切换到Idle状态
            if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
            {
                _complete = true;
                canExit = true;
                return;
            }
        }

        public override void OnExit(PokemonController owner)
        {
            owner.animator.SetBool(PokemonController.attack, false);
            if (!_complete)
            {
                owner.CancelAttack();
            }

            _complete = false;
            canExit = false;
        }
    }
}