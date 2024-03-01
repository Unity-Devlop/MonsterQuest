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
        public NetworkIdentity pokemonIdentity { get; private set; }

        public void InitPokemon(GameObject pokemonGameObj, Vector3 position)
        {
            pokemonTransform = pokemonGameObj.transform;
            pokemonIdentity = pokemonGameObj.GetComponent<NetworkIdentity>();
            Transform model = pokemonGameObj.transform.Find("Model");

            _characterController = pokemonGameObj.GetComponent<CharacterController>();
            _characterController.transform.position = position;

            animator = model.GetComponent<Animator>();
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
            stateMachine.Change<PokemonIdleState>();
            RpcIdle();
        }
        [ClientRpc]
        private void RpcIdle()
        {
            stateMachine.Change<PokemonIdleState>();
        }


        [Command]
        internal void CmdWalk(Vector3 moveVec)
        {
            _characterController.Move(moveVec);
            stateMachine.Change<PokemonWalkState>();
            RpcWalk();
        }

        [ClientRpc]
        private void RpcWalk()
        {
            stateMachine.Change<PokemonWalkState>();
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
        public void CmdRotate(float deltaAngel)
        {
            pokemonTransform.Rotate(Vector3.up, deltaAngel);
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