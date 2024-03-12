using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonAttackState : State<PokemonController>, ITempAnimState
    {
        public bool canExit { get; private set; }

        public override void OnEnter(PokemonController owner)
        {
            owner.animator.SetBool(PokemonController.attack, true);
            canExit = false; // TODO 必须要打出关键帧后才能退出
            owner.hit.Reset();
        }


        public override void OnUpdate(PokemonController owner)
        {
            AnimatorStateInfo stateInfo = owner.animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Attack")) return;

            // TODO 必须要打出关键帧后才能退出
            if (owner.isOwned)
            {
                // Debug.Log("Owned OnUpdate");
                // 攻击检测只在拥有者的客户端进行
                owner.hit.Tick(owner);
            }


            // 动画放结束就切换到Idle状态
            if (stateInfo.normalizedTime > 0.95f)
            {
                // Debug.Log("Player Over");
                canExit = true;
                return;
            }
        }


        public override void OnExit(PokemonController owner)
        {
            // Debug.Log("PokemonAttackState OnExit");
            owner.animator.SetBool(PokemonController.attack, false);
            canExit = false;
        }
    }
}