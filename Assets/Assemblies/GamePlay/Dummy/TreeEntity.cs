using System;
using Cysharp.Threading.Tasks;
using Mirror;
using UnityEngine;

namespace Game
{
    public class TreeEntity : NetworkBehaviour, IHittable
    {
        public int groupId => TeamGroup.Default.id;
        [field: SyncVar, SerializeField] public bool canBeHit { get; set; } = true;


        public int maxHitPoint = 5;
        public int currentHitPoint;

        private void Awake()
        {
            currentHitPoint = maxHitPoint;
        }

        [Command(requiresAuthority = false)]
        public async void CmdBeAttack(int damagePoint, NetworkConnectionToClient sender = null)
        {
            canBeHit = false;
            await UniTask.DelayFrame(10);
            sender.identity.GetComponent<Player>().TargetAddScore(sender,1);
            canBeHit = true;
            currentHitPoint -= damagePoint;
            RpcBeHit(currentHitPoint);
            if (currentHitPoint <= 0)
            {
                NetworkServer.Destroy(gameObject); // TODO Pooling this object
            }
        }


        [ClientRpc]
        private void RpcBeHit(int currentHitPoint)
        {
            Debug.Log($"Tree Hit! Current Hit Point: {currentHitPoint}");
            this.currentHitPoint = currentHitPoint;
        }
    }
}