using System;
using Mirror;
using UnityEngine;

namespace Game
{
    public class HealthComponent : NetworkBehaviour, IHittable
    {
        public event Action<int> OnTakeDamage;

        [Command(requiresAuthority = false)]
        public void CmdBeAttack(int damagePoint)
        {
            Debug.Log("CmdBeAttack");
            OnTakeDamage?.Invoke(damagePoint);
        }
    }
}