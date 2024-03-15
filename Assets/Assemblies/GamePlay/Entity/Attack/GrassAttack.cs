using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Mirror;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class GrassAttack : NetworkBehaviour, IAttackEntity
    {
        private Transform _model;
        private Animator _animator;
        private TrailRenderer _trailRenderer;
        private IEntityController _owner;
        private HashSet<Collider> _filter;
        public int groupId => _owner.groupId;

        private bool _init;
        private Transform _transform;

        private bool _awake;

        private void Awake()
        {
            _transform = transform;
            _filter = new HashSet<Collider>(8);
            _model = transform.Find("Model");
            _trailRenderer = _model.GetComponent<TrailRenderer>();
            _animator = _model.GetComponent<Animator>();
            _trailRenderer.enabled = false;

            _awake = true;
        }

        public async void Setup(IEntityController owner)
        {
            if (!_awake)
            {
                Awake();
            }

            // Debug.Log($"{nameof(GrassAttack)}.Setup");
            _filter.Clear();
            _owner = owner;

            Transform ownerTransform = owner.GetTransform();
            _transform.position = ownerTransform.position;
            _transform.forward = ownerTransform.forward;
            _animator.SetTrigger(IEntity.attack);
            await UniTask.DelayFrame(10); // 动画切换需要一定时间
            _init = true;
            _trailRenderer.enabled = true;
            await UniTask.WaitForSeconds(0.3f);
            _trailRenderer.enabled = false;
            if (isOwned) CmdDestroy();
        }

        [Command]
        private void CmdDestroy()
        {
            NetworkServer.Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if (!_init) return;
            if (isOwned)
            {
                Vector3 center = _model.position;
                DebugDrawer.DrawWireCube(center, Vector3.one * 0.3f, Color.red, 0.1f);
                int count = RayCaster.OverlapBoxAll(center, Vector3.one * 0.3f / 2, out var results,
                    GlobalManager.Singleton.hittableLayer);
                for (int i = 0; i < count; i++)
                {
                    Collider tar = results[i];
                    if (_filter.Contains(tar)) continue;
                    if (tar == _owner.GetCollider()) continue;
                    if (!tar.TryGetComponent(out ICanTakeDamage hittable)) continue;
                    AttackHandler.HandleAttack(_owner, hittable); 
                    _filter.Add(tar);
                }
            }
        }
    }
}