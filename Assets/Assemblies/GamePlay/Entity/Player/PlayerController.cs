using System;
using Game.UI;
using Mirror;
using UnityEngine;

namespace Game
{
    public class PlayerController : NetworkBehaviour, IEntityController
    {
        // AnimHash
        public Animator animator { get; private set; }
        public CharacterController characterController { get; private set; }
        public PlayerStateMachine stateMachine { get; private set; }
        public PokemonData data { get; private set; }
        public Transform modelTransform { get; private set; }

        public Transform orientation { get; private set; }

        // public PlayerHitController hit { get; private set; }

        [field: SerializeField, Sirenix.OdinInspector.ReadOnly]
        public Player player { get; private set; }

        public PackageData package => player.package;

        public int GetDamagePoint()
        {
            return data.damagePoint;
        }

        public Collider GetCollider()
        {
            return characterController;
        }

        public Transform GetTransform()
        {
            return transform;
        }

        public NetworkIdentity GetNetworkIdentity()
        {
            return netIdentity;
        }

        public bool canBeHit { get; set; } = true;

        public int groupId
        {
            get
            {
                if (player == null)
                {
                    return TeamGroup.Default.id;
                }

                return player.data.groupId;
            }
        }

        public Vector3 position => transform.position;

        private bool _init;

        #region Setup

        public void Init(Player player, PokemonData data, Vector3 position)
        {
            this.player = player;
            modelTransform = transform.Find("Model");
            orientation = transform.Find("Orientation");

            characterController = GetComponent<CharacterController>();
            characterController.transform.position = position;

            this.data = data;

            animator = modelTransform.GetComponent<Animator>();

            // hit = new PlayerHitController(this);

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

        public override void OnStopClient()
        {
            _init = false;
        }

        public override void OnStopServer()
        {
            _init = false;
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
            stateMachine = transform.Find("StateMachine").GetComponent<PlayerStateMachine>();
            stateMachine.Setup(this);
        }

        private void HostSetup()
        {
            animator.enabled = true;
            modelTransform.gameObject.SetActive(true);
            stateMachine = transform.Find("StateMachine").GetComponent<PlayerStateMachine>();
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
                if (!characterController.isGrounded) //&& characterController.transform.position.y > 0)
                {
                    // TODO 变成加速运动
                    characterController.Move(Time.deltaTime * Physics.gravity);
                }
            }
        }

        #endregion

        #region Handle

        public void HandleBeAttack(int damagePoint, NetworkConnectionToClient sender = null)
        {
            Debug.Log($"canBeHit:{canBeHit}");
            CmdBeAttack(damagePoint, sender);
        }

        [Command(requiresAuthority = false)]
        private void CmdBeAttack(int damagePoint, NetworkConnectionToClient sender = null)
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
                GlobalManager.EventSystem.Send(new OnLocalPlayerHealthChange()
                {
                    currentHealth = currentHealth,
                    maxHealth = data.maxHealth
                });
            }

            if (NetworkClient.ready)
            {
                if (stateMachine != null)
                {
                    stateMachine.ToBeAttack(this);
                }
            }
        }

        public void HandleIdle()
        {
            if (stateMachine.ToIdle(this))
            {
                CmdIdle();
            }
        }

        [Command]
        internal void CmdIdle()
        {
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
                        stateMachine.ToIdle(this);
                    }
                }
            }
        }

        public void HandleWalk(Vector3 viewDir, Vector2 moveInput)
        {
            if (stateMachine.ToWalk(this))
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
            Vector3 forward = Vector3.Slerp(transform.forward, moveVec.normalized,
                Time.fixedDeltaTime * data.rotateSpeed);
            transform.forward = forward;
            characterController.Move(moveVec * (data.moveSpeed * Time.fixedDeltaTime));

            RpcWalkAnim();
        }

        [ClientRpc]
        private void RpcWalkAnim()
        {
            if (NetworkClient.ready)
            {
                if (!isOwned)
                {
                    stateMachine.ToWalk(this);
                }
            }
        }


        public void HandleRun(Vector3 viewDir, Vector2 moveInput)
        {
            Vector3 moveDir = ProcessMoveInput(viewDir, moveInput);
            if (stateMachine.ToRun(this))
            {
                CmdRun(moveDir);
            }
        }


        [Command]
        private void CmdRun(Vector3 moveVec)
        {
            Vector3 forward =
                Vector3.Slerp(transform.forward, moveVec.normalized, Time.fixedDeltaTime * data.rotateSpeed);
            transform.forward = forward;
            characterController.Move(moveVec * (data.runSpeed * Time.fixedDeltaTime));

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
                        stateMachine.ToRun(this);
                }
            }
        }

        public void HandleAttack()
        {
            // stateMachine.ToAttack(this); // 本地立刻切换状态 避免异常
            CmdAttack();
        }


        [Command]
        private void CmdAttack()
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
                    stateMachine.ToAttack(this);
                }
            }
        }

        #endregion

        private GameObject GetFAttackPrefab()
        {
            return GlobalManager.Singleton.configTable.grassAttackPrefab;
        }

        [Command]
        public void CmdSpawnFAttackEntity()
        {
            GameObject obj = Instantiate(GetFAttackPrefab(), transform.position,
                Quaternion.identity); // TODO 根据玩家状态动态选取
            NetworkServer.Spawn(obj, connectionToClient);
            RpcSpawnFAttackEntity(obj.GetComponent<NetworkIdentity>());
        }


        [ClientRpc]
        private void RpcSpawnFAttackEntity(NetworkIdentity identity)
        {
            IAttackEntity attackEntity = identity.GetComponent<IAttackEntity>();
            attackEntity.Setup(this);
        }
    }
}