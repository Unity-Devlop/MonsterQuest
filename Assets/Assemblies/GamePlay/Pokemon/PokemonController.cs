using System;
using System.Collections.Generic;
using Assemblies;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;
using UnityToolkit;

namespace Game
{
    public class PokemonController : NetworkBehaviour, IHittable
    {
        // AnimHash
        public static readonly int idle = Animator.StringToHash("Idle");
        public static readonly int walk = Animator.StringToHash("Walk");
        public static readonly int run = Animator.StringToHash("Run");
        public static readonly int attack = Animator.StringToHash("Attack");
        public static readonly int BeAttack = Animator.StringToHash("BeAttack");
        public Animator animator { get; private set; }

        public CharacterController characterController { get; private set; }

        public Vector3 pokemonPosition => characterController.transform.position;
        public PokemonStateMachine stateMachine { get; private set; }
        public PokemonData data { get; private set; }

        public Transform pokemonTransform { get; private set; }
        public Transform modelTransform { get; private set; }
        public NetworkIdentity pokemonIdentity { get; private set; }


        // private List<Timer> _attackTimers;

        public Transform orientation { get; private set; }

        public BoxCollider hitBox { get; private set; }
        // private bool _init = false;

        public void InitPokemon(GameObject pokemonGameObj, PokemonData data, Vector3 position)
        {
            pokemonTransform = pokemonGameObj.transform;
            pokemonIdentity = pokemonGameObj.GetComponent<NetworkIdentity>();
            modelTransform = pokemonGameObj.transform.Find("Model");
            orientation = pokemonGameObj.transform.Find("Orientation");

            characterController = pokemonGameObj.GetComponent<CharacterController>();
            characterController.transform.position = position;


            this.data = data;

            animator = modelTransform.GetComponent<Animator>();
            hitBox = modelTransform.Find("HitBox").GetComponent<BoxCollider>();


            // TODO 服务器 和 客户端的区分
            animator.enabled = true;
            // 客户端需要渲染模型
            modelTransform.gameObject.SetActive(true);
            // 客户端需要状态机
            stateMachine = new PokemonStateMachine(this);
            stateMachine.Add<PokemonIdleState>();
            stateMachine.Add<PokemonWalkState>();
            stateMachine.Add<PokemonRunState>();
            stateMachine.Add<PokemonAttackState>();
            stateMachine.Add<PokemonBeAttackState>();
            stateMachine.Start<PokemonIdleState>();
        }

        private void ServerOnlySetup()
        {
        }

        private void ClientOnlySetup()
        {
        }

        private void HostSetup()
        {
        }

        private void Update()
        {
            // TODO Server Only 不需要更新状态机
            if (stateMachine != null)
            {
                stateMachine.OnUpdate();
            }

            if (isServer)
            {
                //不在地面上时，应用重力
                if (!characterController.isGrounded)
                {
                    // TODO 这个不是加速运动
                    characterController.Move(Time.deltaTime * Physics.gravity);
                }
            }
        }


        public void HandleIdle()
        {
            stateMachine.Change<PokemonIdleState>();
            CmdIdle();
        }

        [Command]
        internal void CmdIdle()
        {
            stateMachine.Change<PokemonIdleState>(); // 服务器上没必要播动画
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
                        stateMachine.Change<PokemonIdleState>();
                    }
                }
            }
        }

        internal void HandleWalk(Vector3 viewDir, Vector2 moveInput)
        {
            Vector3 moveDir = ProcessMoveInput(viewDir, moveInput);
            // if (stateMachine.CurrentState is not PokemonWalkState)
            // {
            stateMachine.Change<PokemonWalkState>();
            // }
            CmdWalk(moveDir);
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
            pokemonTransform.forward =
                Vector3.Slerp(pokemonTransform.forward, moveVec.normalized, Time.deltaTime * data.rotateSpeed);
            HandleCharacterControllerMove(moveVec * (Time.deltaTime * data.moveSpeed));
            stateMachine.Change<PokemonWalkState>(); // 服务器上没必要播动画
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
                    stateMachine.Change<PokemonWalkState>();
                    // }
                }
            }
        }


        public void HandleRun(Vector3 viewDir, Vector2 moveInput)
        {
            Vector3 moveDir = ProcessMoveInput(viewDir, moveInput);
            stateMachine.Change<PokemonRunState>();
            CmdRun(moveDir);
        }

        private void HandleCharacterControllerMove(Vector3 moveVec)
        {
            characterController.Move(moveVec);
        }

        [Command]
        private void CmdRun(Vector3 moveVec)
        {
            pokemonTransform.forward =
                Vector3.Slerp(pokemonTransform.forward, moveVec.normalized, Time.deltaTime * data.rotateSpeed);
            HandleCharacterControllerMove(moveVec * (Time.deltaTime * data.runSpeed));
            stateMachine.Change<PokemonRunState>(); // 服务器上没必要播动画
            RpcRunAnim();
        }

        [ClientRpc]
        private void RpcRunAnim()
        {
            if (NetworkClient.ready)
            {
                if (!isOwned)
                {
                    /*if (_init)
                    {*/
                    if (stateMachine != null)
                        stateMachine.Change<PokemonRunState>();
                    // }
                }
            }
        }


        public void HandleAttack()
        {
            stateMachine.Change<PokemonAttackState>();
            CmdAttack();
        }

        [Command]
        private void CmdAttack()
        {
            // for (int i = 0; i < data.config.hitBoxFrames.Count; i++)
            // {
            //     HitBoxFrame hitBoxFrame = data.config.hitBoxFrames[i];
            //     int idx = i;
            //     Timer timer = this.AttachTimer(hitBoxFrame.time, () => { OnAttack(idx); });
            //     _attackTimers.Add(timer);
            // }

            // Host 已经播了  所以不需要再播 仅服务器时 这个动画需要播
            if (isServerOnly)
            {
                stateMachine.Change<PokemonAttackState>();
            }

            RpcAttackAnim();
        }

        [ClientRpc]
        private void RpcAttackAnim()
        {
            if (NetworkClient.ready)
            {
                if (!isOwned)
                {
                    if (stateMachine != null)
                        // Debug.Log("RpcAttackAnim");
                        stateMachine.Change<PokemonAttackState>();
                }
            }
        }

        [Server]
        private void OnAttack()
        {
            // int count = RayCaster.OverlapSphereAll(pokemonTransform.position + frame.center, frame.radius,
            //     out var results, GlobalManager.Singleton.hittableLayer);
            // for (int i = 0; i < count; i++)
            // {
            //     if (results[i].gameObject.TryGetComponent(out IHittable hittable) && hittable.CanBeHit() &&
            //         !ReferenceEquals(hittable, this))
            //     {
            //         int damagePoint = data.damagePoint;
            //         hittable.CmdBeAttack(damagePoint);
            //     }
            // }
        }


        public bool CanBeHit()
        {
            return true; // TODO complete this method
        }


        [Command]
        public void CmdBeAttack(int damagePoint)
        {
            data.currentHealth -= damagePoint;
            RpcBeAttack(data.currentHealth);
        }

        [ClientRpc]
        private void RpcBeAttack(int currentHealth)
        {
            data.currentHealth = currentHealth;
            if (NetworkClient.ready)
            {
                if (!isOwned)
                {
                    if (stateMachine != null)
                    {
                        stateMachine.Change<PokemonBeAttackState>();
                    }
                }
            }
        }
    }
}