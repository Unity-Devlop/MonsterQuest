﻿using System;
using Game.UI;
using Mirror;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonController : NetworkBehaviour, IHittable
    {
        // AnimHash
        public Animator animator { get; private set; }
        public CharacterController characterController { get; private set; }
        public Vector3 pokemonPosition => characterController.transform.position;
        public PokemonStateMachine stateMachine { get; private set; }
        public PokemonData data { get; private set; }

        public Transform pokemonTransform { get; private set; }
        public Transform modelTransform { get; private set; }
        public NetworkIdentity pokemonIdentity { get; private set; }


        public Transform orientation { get; private set; }

        public HitController hit { get; private set; }

        private bool _init;

        [field: SerializeField, Sirenix.OdinInspector.ReadOnly]
        public Player player { get; private set; }

        public bool canBeHit { get; set; } = true;

        public int groupId
        {
            get
            {
                if (player == null)
                {
                    return TeamGroup.Default.id;
                }

                return player.data.group.id;
            }
        }

        public void Init(Player player, GameObject obj, PokemonData data, Vector3 position)
        {
            this.player = player;
            pokemonTransform = obj.transform;
            pokemonIdentity = obj.GetComponent<NetworkIdentity>();
            modelTransform = obj.transform.Find("Model");
            orientation = obj.transform.Find("Orientation");

            characterController = obj.GetComponent<CharacterController>();
            characterController.transform.position = position;

            this.data = data;

            animator = modelTransform.GetComponent<Animator>();

            hit = modelTransform.Find("HitController").GetComponent<HitController>();

            NetworkManagerMode mode = NetworkManager.singleton.mode;
            if (mode == NetworkManagerMode.ServerOnly)
            {
                ServerOnlySetup();
            }
            else if (mode == NetworkManagerMode.ClientOnly)
            {
                ClientOnlySetup();
            }
            else if (mode == NetworkManagerMode.Host)
            {
                HostSetup();
            }
            else
            {
                throw new NotImplementedException($"未实现的网络模式处理:{mode}");
            }

            _init = true;
        }


        private void ServerOnlySetup()
        {
            animator.enabled = false;
            modelTransform.gameObject.SetActive(false);
            Destroy(transform.Find("StateMachine").gameObject);
        }

        private void ClientOnlySetup()
        {
            animator.enabled = true;
            modelTransform.gameObject.SetActive(true);
            stateMachine = transform.Find("StateMachine").GetComponent<PokemonStateMachine>();
            stateMachine.Setup(this);
        }

        private void HostSetup()
        {
            animator.enabled = true;
            modelTransform.gameObject.SetActive(true);
            stateMachine = transform.Find("StateMachine").GetComponent<PokemonStateMachine>();
            stateMachine.Setup(this);
        }

        private void Update()
        {
            if (!_init) return;
            if (!isServerOnly)
            {
                // TODO Server Only 不需要更新状态机
                if (stateMachine != null)
                {
                    stateMachine.OnUpdate(this);
                }
            }


            if (isServer)
            {
                //不在地面上时，应用重力
                if (!characterController.isGrounded && characterController.transform.position.y > 0)
                {
                    // TODO 变成加速运动
                    characterController.Move(Time.deltaTime * Physics.gravity);
                }
            }
        }


        [Command(requiresAuthority = false)]
        public void CmdBeAttack(int damagePoint, NetworkConnectionToClient sender = null)
        {
            data.currentHealth -= damagePoint;
            // OnCurrentHealthChanged?.Invoke(data.currentHealth);
            RpcBeAttack(data.currentHealth);
        }


        [ClientRpc]
        private void RpcBeAttack(int currentHealth)
        {
            data.currentHealth = currentHealth;
            if (player != null && player.isLocalPlayer)
            {
                GlobalManager.EventSystem.Send(new OnLocalPlayerPokemonHealthChange()
                {
                    currentHealth = currentHealth,
                    maxHealth = data.maxHealth
                });
            }

            // OnCurrentHealthChanged?.Invoke(data.currentHealth, data.maxHealth);
            if (NetworkClient.ready)
            {
                if (stateMachine != null)
                {
                    stateMachine.Change<PokemonBeAttackState>(this);
                }
            }
        }

        public void HandleIdle()
        {
            if (stateMachine.Change<PokemonIdleState>(this))
            {
                CmdIdle();
            }

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
                    if (stateMachine != null)
                    {
                        stateMachine.Change<PokemonIdleState>(this);
                    }
                }
            }
        }

        internal void HandleWalk(Vector3 viewDir, Vector2 moveInput)
        {
            if (stateMachine.Change<PokemonWalkState>(this))
            {
                Vector3 moveDir = ProcessMoveInput(viewDir, moveInput);
                CmdWalk(moveDir);
            }
        }

        private Vector3 ProcessMoveInput(Vector3 viewDir, Vector2 moveInput)
        {
            orientation.forward = viewDir.normalized;
            float forward = moveInput.y;
            float right = moveInput.x;
            Vector3 moveDir = orientation.forward * forward +
                              orientation.right * right;
            moveDir.y = 0;
            return moveDir;
        }

        [Command]
        internal void CmdWalk(Vector3 moveVec)
        {
            Vector3 forward = Vector3.Slerp(pokemonTransform.forward, moveVec.normalized,
                Time.deltaTime * data.rotateSpeed);
            pokemonTransform.forward = forward;
            characterController.Move(moveVec * (data.moveSpeed * Time.deltaTime));

            RpcWalkAnim();
        }

        [ClientRpc]
        private void RpcWalkAnim()
        {
            if (NetworkClient.ready)
            {
                if (!isOwned)
                {
                    /*if (_init)
                    {*/
                    stateMachine.Change<PokemonWalkState>(this);
                    // }
                }
            }
        }


        public void HandleRun(Vector3 viewDir, Vector2 moveInput)
        {
            Vector3 moveDir = ProcessMoveInput(viewDir, moveInput);
            if (stateMachine.Change<PokemonRunState>(this))
            {
                CmdRun(moveDir);
            }
        }


        [Command]
        private void CmdRun(Vector3 moveVec)
        {
            Vector3 forward =
                Vector3.Slerp(pokemonTransform.forward, moveVec.normalized, Time.deltaTime * data.rotateSpeed);
            pokemonTransform.forward = forward;
            characterController.Move(moveVec * (data.runSpeed * Time.deltaTime));

            RpcRunAnim();
        }

        [ClientRpc]
        private void RpcRunAnim()
        {
            if (NetworkClient.ready)
            {
                if (!isOwned)
                {
                    if (stateMachine != null)
                        stateMachine.Change<PokemonRunState>(this);
                }
            }
        }

        public void HandleAttack()
        {
            stateMachine.Change<PokemonAttackState>(this); // 本地立刻切换状态 避免异常
            CmdAttack();
        }

        [Command]
        public void CmdAttack()
        {
            RpcAttackAnim();
        }

        [ClientRpc]
        private void RpcAttackAnim()
        {
            if (NetworkClient.ready)
            {
                if (stateMachine != null)
                {
                    if (!isOwned)
                    {
                        stateMachine.Change<PokemonAttackState>(this);
                    }
                }
            }
        }
    }
}