using System;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class DummyEntity : NetworkBehaviour, IHittable
    {
        [field: SyncVar, SerializeField] public bool canBeHit { get; set; } = true;

        [Command(requiresAuthority = false)]
        public async void CmdBeAttack(int damagePoint, NetworkConnectionToClient sender = null)
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