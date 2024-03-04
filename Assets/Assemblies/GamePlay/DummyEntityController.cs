using System;
using Mirror;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class DummyEntityController : NetworkBehaviour, IHittable
    {
        private bool canBeHit = true;
        private Timer _hitCoolDown;
        public int maxHealth = 100;

        [Sirenix.OdinInspector.ReadOnly, SerializeField]
        private int currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        [Command(requiresAuthority = false)]
        public void CmdBeAttack(int damagePoint)
        {
            if (!canBeHit) return;

            Debug.Log($"CmdBeAttack:{damagePoint}");
            canBeHit = false;
            _hitCoolDown = this.AttachTimer(1, () => { canBeHit = true; });
            currentHealth -= damagePoint;
            RpcSetHealth(currentHealth);
        }

        [ClientRpc]
        private void RpcSetHealth(int health)
        {
            currentHealth = health;
            Debug.Log($"RpcSetHealth:{health}");
        }
    }
}