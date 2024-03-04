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
                        if (!collider.TryGetComponent(out IHittable hittable)) continue;
                        _filter.Add(collider);
                        // TODO 目标过滤 组队模式下只能攻击敌方

                        if (owner.isWild) // 野生宝可梦可以随便攻击
                        {
                            Debug.Log("Wild Pokemon Attack");
                            hittable.CmdBeAttack(owner.data.damagePoint);
                            return;
                        }
                        Debug.Log($"HitTarget GroupId:{hittable.GroupId()} Owner GroupId:{owner.player.data.group.id}");
                        
                        // 攻击者 或者 被攻击者是默认组的话就可以攻击
                        if (hittable.GroupId() == TeamGroup.Default.id || owner.groupId == TeamGroup.Default.id)
                        {
                            Debug.Log("Default Group Attack");
                            hittable.CmdBeAttack(owner.data.damagePoint);
                            return;
                        }

                        // 如果不是默认组 且 组不同 则可以攻击
                        if (hittable.GroupId() != owner.player.data.group.id)
                        {
                            Debug.Log("Different Group Attack");
                            hittable.CmdBeAttack(owner.data.damagePoint);
                            return;
                        }

                        Debug.Log("Same Group Can't Attack");
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