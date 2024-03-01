using System;
using Cinemachine;
using MemoryPack;
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

        public void InitComponent(GameObject pokemonGameObj)
        {
            _characterController = pokemonGameObj.GetComponent<CharacterController>();
            Transform model = pokemonGameObj.transform.Find("Model");
            _animator = model.GetComponent<Animator>();
        }
        
        [Command]
        private void CmdRun(Vector3 moveVec)
        {
        }

        [Command]
        private void CmdWalk()
        {
        }

        [Command]
        private void CmdAttack()
        {
        }

        [Command]
        private void CmdBeAttack()
        {
        }

        [Command]
        private void CmdSkill()
        {
        }
    }
}