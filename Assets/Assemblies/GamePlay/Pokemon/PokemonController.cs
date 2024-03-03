using System;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using UnityToolkit;

namespace Game
{
    public class PokemonController : NetworkBehaviour
    {
        // AnimHash
        public static readonly int idle = Animator.StringToHash("Idle");
        public static readonly int walk = Animator.StringToHash("Walk");
        public static readonly int run = Animator.StringToHash("Run");
        public static readonly int attack = Animator.StringToHash("Attack");
        public static readonly int BeAttack = Animator.StringToHash("BeAttack");
        public Animator animator { get; private set; }

        private CharacterController _characterController;

        public Vector3 pokemonPosition => _characterController.transform.position;
        public PokemonStateMachine stateMachine { get; private set; }

        public Transform pokemonTransform { get; private set; }
        public Transform modelTransform { get; private set; }
        public NetworkIdentity pokemonIdentity { get; private set; }
        
        public Transform orientation { get; private set; }

        public void InitPokemon(GameObject pokemonGameObj, Vector3 position)
        {
            pokemonTransform = pokemonGameObj.transform;
            pokemonIdentity = pokemonGameObj.GetComponent<NetworkIdentity>();
            modelTransform = pokemonGameObj.transform.Find("Model");
            orientation = pokemonGameObj.transform.Find("Orientation");

            _characterController = pokemonGameObj.GetComponent<CharacterController>();
            _characterController.transform.position = position;

            animator = modelTransform.GetComponent<Animator>();
            stateMachine = new PokemonStateMachine(this);
            stateMachine.Add<PokemonIdleState>();
            stateMachine.Add<PokemonWalkState>();
            stateMachine.Add<PokemonRunState>();
            stateMachine.Add<PokemonAttackState>();
            stateMachine.Add<PokemonBeAttackState>();
            stateMachine.Start<PokemonIdleState>();
        }

        [Command]
        internal void CmdRun(Vector3 moveVec)
        {
        }

        [Command]
        internal void RpcRun(Vector3 moveVec)
        {
            _characterController.Move(moveVec);
        }


        [Command]
        internal void CmdIdle()
        {
            // stateMachine.Change<PokemonIdleState>(); // 服务器上没必要播动画
            RpcIdle();
        }
        [ClientRpc]
        private void RpcIdle()
        {
            if (NetworkClient.ready)
            {
                if (!isOwned)
                {
                    stateMachine.Change<PokemonIdleState>();
                }
            }
        }

        // TODO remove moveSpeed param
        internal void HandleWalk(Vector3 viewDir,Vector2 moveInput,float moveSpeed,float rotateSpeed)
        {
            orientation.forward = viewDir.normalized;
            float forward = moveInput.y;
            float right = moveInput.x;
            // Vector3 viewDir 
            
            // 计算Vec
            Vector3 moveVec = orientation.forward * forward +
                              orientation.right * right;
            moveVec *= moveSpeed * Time.deltaTime;
            
            stateMachine.Change<PokemonWalkState>();
            
            CmdWalk(moveVec,rotateSpeed);
        }

        [Command]
        internal void CmdWalk(Vector3 moveVec,float rotateSpeed)
        {
            pokemonTransform.forward = Vector3.Slerp(pokemonTransform.forward, moveVec.normalized,Time.deltaTime*rotateSpeed);
            _characterController.Move(moveVec);
            // stateMachine.Change<PokemonWalkState>(); // 服务器上没必要播动画
            RpcWalk();
        }

        [ClientRpc]
        private void RpcWalk()
        {
            if (NetworkClient.ready)
            {
                if (!isOwned)
                {
                    stateMachine.Change<PokemonWalkState>();
                }
            }
        }


        [Command]
        internal void CmdAttack()
        {
        }

        [Command]
        internal void CmdBeAttack()
        {
        }

        // [ClientRpc]
        // public void RpcSetAnimBool(int hash, bool value)
        // {
        //     animator.SetBool(hash, value);
        // }
        //
        // [Command]
        // public void CmdSetAnimBool(int hash, bool value)
        // {
        //     RpcSetAnimBool(hash, value);
        // }
    }
}