using Assemblies;
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
            if (owner.isServer)
            {
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
                        if (collider.TryGetComponent(out IHittable hittable) && hittable.CanBeHit())
                        {
                            hittable.CmdBeAttack(owner.data.damagePoint);
                        }
                    }
                }
            }


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
            _complete = false;
            canExit = false;
        }
    }
}