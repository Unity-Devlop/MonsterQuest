using System;
using System.Collections.Generic;
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

        public CharacterController characterController { get; private set; }
        public HitTarget hitTarget { get; private set; }

        public Vector3 pokemonPosition => characterController.transform.position;
        public PokemonStateMachine stateMachine { get; private set; }
        public PokemonData data { get; private set; }

        public Transform pokemonTransform { get; private set; }
        public Transform modelTransform { get; private set; }
        public NetworkIdentity pokemonIdentity { get; private set; }


        public Transform orientation { get; private set; }

        public BoxCollider hitBox { get; private set; }

        // private bool _init = false;
        [field: SerializeField] public PlayerController player { get; private set; }
        public bool isWild => player == null;

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

        public void InitPokemon(PlayerController ownerId, GameObject pokemonGameObj, PokemonData data, Vector3 position)
        {
            this.player = ownerId;
            pokemonTransform = pokemonGameObj.transform;
            pokemonIdentity = pokemonGameObj.GetComponent<NetworkIdentity>();
            modelTransform = pokemonGameObj.transform.Find("Model");
            orientation = pokemonGameObj.transform.Find("Orientation");

            characterController = pokemonGameObj.GetComponent<CharacterController>();
            characterController.transform.position = position;

            hitTarget = pokemonGameObj.GetComponent<HitTarget>();
            if (isServer)
            {  
                hitTarget.ServerSetGroupId(player.data.group.id);
            }
            else
            {
                hitTarget.CmdSetGroupId(player.data.group.id);
            }
            hitTarget.OnTakeDamage += OnTakeDamage;


            this.data = data;

            animator = modelTransform.GetComponent<Animator>();
            hitBox = modelTransform.Find("HitBox").GetComponent<BoxCollider>();


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
        }


        private void ServerOnlySetup()
        {
            animator.enabled = false;
            modelTransform.gameObject.SetActive(false);
        }

        private void ClientOnlySetup()
        {
            animator.enabled = true;
            modelTransform.gameObject.SetActive(true);
            stateMachine = new PokemonStateMachine(this);
            stateMachine.Add<PokemonIdleState>();
            stateMachine.Add<PokemonWalkState>();
            stateMachine.Add<PokemonRunState>();
            stateMachine.Add<PokemonAttackState>();
            stateMachine.Add<PokemonBeAttackState>();
            stateMachine.Start<PokemonIdleState>();
        }

        private void HostSetup()
        {
            animator.enabled = true;
            modelTransform.gameObject.SetActive(true);
            stateMachine = new PokemonStateMachine(this);
            stateMachine.Add<PokemonIdleState>();
            stateMachine.Add<PokemonWalkState>();
            stateMachine.Add<PokemonRunState>();
            stateMachine.Add<PokemonAttackState>();
            stateMachine.Add<PokemonBeAttackState>();
            stateMachine.Start<PokemonIdleState>();
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
                if (!characterController.isGrounded && characterController.transform.position.y > 0)
                {
                    // TODO 这个不是加速运动
                    characterController.Move(Time.deltaTime * Physics.gravity);
                }
            }
        }


        [Server]
        private void OnTakeDamage(int damagePoint)
        {
            // Debug.Log("OnTakeDamage");
            data.currentHealth -= damagePoint;
            RpcBeAttack(data.currentHealth);
        }

        [ClientRpc]
        private void RpcBeAttack(int currentHealth)
        {
            data.currentHealth = currentHealth;
            if (NetworkClient.ready)
            {
                if (stateMachine != null)
                {
                    stateMachine.Change<PokemonBeAttackState>();
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
            // stateMachine.Change<PokemonWalkState>(); // 服务器上没必要播动画
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
            // stateMachine.Change<PokemonRunState>(); // 服务器上没必要播动画
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
                        stateMachine.Change<PokemonRunState>();
                }
            }
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
                        stateMachine.Change<PokemonAttackState>();
                    }
                }
            }
        }
    }
}