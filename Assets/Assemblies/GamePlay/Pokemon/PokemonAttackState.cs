using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonAttackState : State<PokemonController>, ITempAnimState
    {
        public bool canExit { get; private set; }
        // private AnimationClip targetClip;
        private readonly HashSet<Collider> _filter = new HashSet<Collider>(10);
        public override void OnEnter(PokemonController owner)
        {
            owner.animator.SetBool(PokemonController.attack, true);
            canExit = false; // TODO 必须要打出关键帧后才能退出
            _filter.Clear();
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
                if (owner.hitBox.gameObject.activeSelf)
                {
                    Vector3 center = owner.hitBox.transform.position + owner.hitBox.center;
                    DebugDrawer.DrawWireCube(center, owner.hitBox.size, Color.red);
                    int count = RayCaster.OverlapBoxAll(center, owner.hitBox.size / 2, out var colliders,
                        GlobalManager.Singleton.hittableLayer);
                    for (int i = 0; i < count; i++)
                    {
                        Collider collider = colliders[i];
                        if (collider == owner.characterController) continue;
                        if (_filter.Contains(collider)) continue;
                        if (collider.TryGetComponent(out IHittable hittable))
                        {
                            _filter.Add(collider);
                            hittable.CmdBeAttack(owner.data.damagePoint);
                        }
                    }
                }
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
            _filter.Clear();
            // Debug.Log("PokemonAttackState OnExit");
            owner.animator.SetBool(PokemonController.attack, false);
            canExit = false;
        }
    }
}