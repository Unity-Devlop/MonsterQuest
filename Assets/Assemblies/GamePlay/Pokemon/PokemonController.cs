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
        public PokemonData data { get; private set; }

        public Transform pokemonTransform { get; private set; }
        public Transform modelTransform { get; private set; }
        public NetworkIdentity pokemonIdentity { get; private set; }

        public Transform orientation { get; private set; }
        // private bool _init = false;

        public void InitPokemon(GameObject pokemonGameObj, PokemonData data, Vector3 position)
        {
            pokemonTransform = pokemonGameObj.transform;
            pokemonIdentity = pokemonGameObj.GetComponent<NetworkIdentity>();
            modelTransform = pokemonGameObj.transform.Find("Model");
            orientation = pokemonGameObj.transform.Find("Orientation");

            _characterController = pokemonGameObj.GetComponent<CharacterController>();
            _characterController.transform.position = position;


            this.data = data;

            animator = modelTransform.GetComponent<Animator>();
            
            
            // 服务器 和 客户端的区分
            if (NetworkManager.singleton.mode == NetworkManagerMode.ServerOnly)
            {
                Debug.Log("isServerOnly");
                // 服务器不需要播动画
                animator.enabled = false;
                // 服务器不需要渲染模型
                modelTransform.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("isNotServerOnly");
                // 客户端需要播动画
                animator.enabled = true;
                // 客户端需要状态机
                stateMachine = new PokemonStateMachine(this);
                stateMachine.Add<PokemonIdleState>();
                stateMachine.Add<PokemonWalkState>();
                stateMachine.Add<PokemonRunState>();
                stateMachine.Add<PokemonAttackState>();
                stateMachine.Add<PokemonBeAttackState>();
                stateMachine.Start<PokemonIdleState>();
                // 客户端需要渲染模型
                modelTransform.gameObject.SetActive(true);
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
                    // if (_init)
                    // {
                    stateMachine.Change<PokemonIdleState>();
                    // }
                }
            }
        }

        // TODO remove moveSpeed param
        internal void HandleWalk(Vector3 viewDir, Vector2 moveInput)
        {
            Vector3 moveDir = ProcessMoveInput(viewDir, moveInput);

            stateMachine.Change<PokemonWalkState>();

            CmdWalk(moveDir);
        }

        private Vector3 ProcessMoveInput(Vector3 viewDir, Vector2 moveInput)
        {
            orientation.forward = viewDir.normalized;
            float forward = moveInput.y;
            float right = moveInput.x;
            Vector3 moveDir = orientation.forward * forward +
                              orientation.right * right;
            return moveDir;
        }

        [Command]
        internal void CmdWalk(Vector3 moveVec)
        {
            pokemonTransform.forward =
                Vector3.Slerp(pokemonTransform.forward, moveVec.normalized, Time.deltaTime * data.rotateSpeed);
            _characterController.Move(moveVec * (Time.deltaTime * data.moveSpeed));
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

        [Command]
        private void CmdRun(Vector3 moveVec)
        {
            pokemonTransform.forward =
                Vector3.Slerp(pokemonTransform.forward, moveVec.normalized, Time.deltaTime * data.rotateSpeed);
            _characterController.Move(moveVec * (Time.deltaTime * data.runSpeed));
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
                    /*if (_init)
                    {*/
                    stateMachine.Change<PokemonRunState>();
                    // }
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
    }
}