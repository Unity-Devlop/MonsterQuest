using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PlayerAttackState : IState<PlayerController>//, ITempAnimState
    {
        // public bool canExit { get; private set; }
        // public bool over { get; private set; }

        public void OnEnter(PlayerController owner)
        {
            owner.animator.SetBool(IEntity.attack, true);
            // canExit = false;
            // over = false;
            
            // 根据玩家现在的状态来使用不同的普通攻击技能
        }

        public void OnUpdate(PlayerController owner)
        {
            // AnimatorStateInfo stateInfo = owner.animator.GetCurrentAnimatorStateInfo(0);
            // if (!stateInfo.IsName("Attack")) return;
            // if (stateInfo.normalizedTime >= 0.3f && canExit == false)
            // {
            //     canExit = true;
            // }
            //
            // if (stateInfo.normalizedTime >= 0.95f)
            // {
            //     canExit = true;
            //     over = true;
            //     return;
            // }
        }

        public void OnExit(PlayerController owner)
        {
            owner.animator.SetBool(IEntity.attack, false);
        }
    }
}