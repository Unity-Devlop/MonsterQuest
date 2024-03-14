using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonAttackState : IState<PokemonController>, ITempAnimState
    {
        public bool canExit { get; private set; }
        public bool over { get; private set; }
        private const float MinExitPercent = 0.7f;

        public void OnEnter(PokemonController owner)
        {
            owner.animator.SetBool(IEntity.attack, true);
            canExit = false;
            over = false;
            owner.hit.Reset();
        }


        public void OnUpdate(PokemonController owner)
        {
            AnimatorStateInfo stateInfo = owner.animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Attack")) return;

            if (stateInfo.normalizedTime >= MinExitPercent && !canExit)
            {
                canExit = true;
            }
            
            if (owner.isOwned)
            {
                // 攻击检测只在拥有者的客户端进行
                owner.hit.Tick(owner);
            }
            // 动画放结束就切换到Idle状态
            if (stateInfo.normalizedTime > 0.95f)
            {
                // Debug.Log("Player Over");
                canExit = true;
                over = true;
            }
        }


        public void OnExit(PokemonController owner)
        {
            // Debug.Log("PokemonAttackState OnExit");
            owner.animator.SetBool(IEntity.attack, false);
            canExit = false;
            over = false;
        }
    }
}