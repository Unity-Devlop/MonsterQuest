using System;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class DummyEntity : NetworkBehaviour, ICanTakeDamage
    {
        [field: SyncVar, SerializeField] public bool canBeHit { get; set; } = true;

        public void HandleBeAttack(int damagePoint, NetworkConnectionToClient sender = null)
        {
            CmdBeAttack(damagePoint, sender);
        }

        private async void CmdBeAttack(int damagePoint, NetworkConnectionToClient sender = null)
        {
            RpcBeHit();
            canBeHit = false;
            await UniTask.DelayFrame(1);
            canBeHit = true;
        }

        public int groupId => TeamGroup.Default.id;


        [ClientRpc]
        private void RpcBeHit()
        {
        }
    }
}