using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class GrassAttack : MonoBehaviour,IEntity
    {
        private Transform _model;
        private Animator _animator;
        private TrailRenderer _trailRenderer;
        private PlayerController owner;
        private HashSet<Collider> filter;

        public int groupId =>owner.groupId;
        private void Awake()
        {
            filter = new HashSet<Collider>(8);
            _model = transform.Find("Model");
            _trailRenderer = _model.GetComponent<TrailRenderer>();
            _animator = _model.GetComponent<Animator>();
            _trailRenderer.enabled = false;
        }

        public void Setup(PlayerController owner)
        {
            filter.Clear();
            this.owner = owner;
            _trailRenderer.enabled = true;
            transform.position = owner.transform.position;
            transform.forward = owner.transform.forward;
            _animator.SetTrigger(IEntity.attack);
            Timer.Register(0.4f, () =>
            {
                _trailRenderer.enabled = false;
                Destroy(gameObject);
            });
        }

        private void Update()
        {
            if (!owner.isOwned) return;
            DebugDrawer.DrawWireCube(_model.transform.position, Vector3.one * 0.3f, Color.red, 0.1f);
            int count = RayCaster.OverlapSphereAll(_model.transform.position, 0.3f, out var results,
                GlobalManager.Singleton.hittableLayer);
            for (int i = 0; i < count; i++)
            {
                Collider tar = results[i];
                if (filter.Contains(tar)) continue;
                if (tar == owner.characterController) continue;
                if(!tar.TryGetComponent(out IHittable hittable)) continue;
                AttackHandler.HandleAttack(this, hittable, owner.data.damagePoint);// TODO Cal DamagePoint
                filter.Add(tar);
            }
        }

    }
}