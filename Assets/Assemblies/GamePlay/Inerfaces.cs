using Mirror;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public delegate void OnHealthChangeDelegate(int current, int max);


    public interface IEntity
    {
        public const string Idle = "Idle";
        public const string Walk = "Walk";
        public const string Run = "Run";
        public const string Attack = "Attack";
        public const string BeAttack = "BeAttack";

        public static readonly int idle = Animator.StringToHash(Idle);
        public static readonly int walk = Animator.StringToHash(Walk);
        public static readonly int run = Animator.StringToHash(Run);
        public static readonly int attack = Animator.StringToHash(Attack);
        public static readonly int beAttack = Animator.StringToHash(BeAttack);
    }

    public interface IHaveGroupId
    {
        int groupId { get; }
    }

    public interface IAttackEntity
    {
        void Setup(IEntityController playerController); // TODO Change to IEntityController
    }


    public interface IEntityStateMachine<T> : IStateMachine<T> where T : IEntity
    {
        public bool ToIdle(T owner);
        public bool ToWalk(T owner);
        public bool ToRun(T owner);
        public bool ToAttack(T owner);
        public bool ToBeAttack(T owner);
    }


    public interface ICanTakeDamage : IHaveGroupId
    {
        bool canBeHit { get; set; }
        void HandleBeAttack(int damagePoint, NetworkConnectionToClient sender = null);
    }


    public interface ICanApplyDamage : IHaveGroupId
    {
        int GetDamagePoint();
        Collider GetCollider();
    }

    public interface IEntityController : IEntity, ICanTakeDamage, ICanApplyDamage
    {
        Transform GetTransform();
        NetworkIdentity GetNetworkIdentity();
        void HandleIdle();
        void HandleWalk(Vector3 viewDir, Vector2 moveInput);
        void HandleRun(Vector3 viewDir, Vector2 moveInput);
        void HandleAttack();
    }


    public interface ITempAnimState
    {
        bool canExit { get; }
        bool over { get; }
    }
}