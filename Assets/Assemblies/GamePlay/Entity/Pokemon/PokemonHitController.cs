using System;
using System.Collections.Generic;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonHitController : MonoBehaviour
    {
        protected readonly HashSet<Collider> filter = new HashSet<Collider>(10);

        public void Reset()
        {
            filter.Clear();
        }

        private BoxCollider _hitBox;

        private void Awake()
        {
            _hitBox = GetComponent<BoxCollider>();
        }


        public void Tick(PokemonController owner)
        {
            if (_hitBox.enabled)
            {
                Vector3 center = _hitBox.transform.position + _hitBox.center;
                DebugDrawer.DrawWireCube(center, _hitBox.size, Color.red);
                int count = RayCaster.OverlapBoxAll(center, _hitBox.size / 2, out var colliders,
                    GlobalManager.Singleton.hittableLayer);
                for (int i = 0; i < count; i++)
                {
                    Collider tar = colliders[i];
                    if (tar == owner.characterController) continue;
                    if (filter.Contains(tar)) continue;
                    if (!tar.TryGetComponent(out ICanTakeDamage hittable)) continue;
                    filter.Add(tar);
                    AttackHandler.HandleAttack(owner, hittable);
                }
            }
        }
    }
}