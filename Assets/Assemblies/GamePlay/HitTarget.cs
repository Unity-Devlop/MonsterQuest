using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [RequireComponent(typeof(Collider))]
    public class HitTarget : NetworkBehaviour, IHittable
    {
        public int groupId { get; private set; }
        public event Action<int> OnTakeDamage;


        [ClientRpc]
        private void RpcSetGroupId(int groupId)
        {
            this.groupId = groupId;
        }

        [Server]
        public void ServerSetGroupId(int groupID)
        {
            this.groupId = groupID;
        }

        [Command(requiresAuthority = false)]
        public void CmdSetGroupId(int groupId)
        {
            this.groupId = groupId;
            RpcSetGroupId(groupId);
        }

        public int GroupId()
        {
            return groupId;
        }

        [Command(requiresAuthority = false)]
        public void CmdBeAttack(int damagePoint)
        {
            // Debug.Log("CmdBeAttack");
            OnTakeDamage?.Invoke(damagePoint);
        }
    }
}