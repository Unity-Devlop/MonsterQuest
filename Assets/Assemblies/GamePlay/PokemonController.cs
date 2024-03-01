using Mirror;
using UnityEngine;

namespace Game
{
    public class PokemonController : NetworkBehaviour
    {
        // AnimHash
        private static readonly int idle = Animator.StringToHash("Idle");
        private static readonly int walk = Animator.StringToHash("Walk");
        private static readonly int run = Animator.StringToHash("Run");
        private static readonly int attack = Animator.StringToHash("Attack");
        private static readonly int BeAttack = Animator.StringToHash("BeAttack");

        private Animator _animator;
        private CharacterController _characterController;
        private bool _initialized = false;
        public Vector3 pokemonPosition => _characterController.transform.position;

        public void Init(GameObject pokemonGameObj,Vector3 position)
        {
            _characterController = pokemonGameObj.GetComponent<CharacterController>();
            _characterController.transform.position = position;
            Transform model = pokemonGameObj.transform.Find("Model");
            _animator = model.GetComponent<Animator>();
        }

        [Command]
        internal void CmdRun(Vector3 moveVec)
        {
        }

        [Command]
        internal void CmdWalk(Vector3 moveVec)
        {
            RpcWalk(moveVec);
        }

        [ClientRpc]
        internal void RpcWalk(Vector3 moveVec)
        {
            _characterController.Move(moveVec);
        }

        [Command]
        internal void CmdAttack()
        {
        }

        [Command]
        internal void CmdBeAttack()
        {
        }

        [Command]
        internal void CmdSkill()
        {
        }
    }
}