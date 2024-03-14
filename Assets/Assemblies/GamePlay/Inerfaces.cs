using Mirror;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public delegate void OnHealthChangeDelegate(int current, int max);

    public interface IEntity
    {
        public static readonly int idle = Animator.StringToHash("Idle");
        public static readonly int walk = Animator.StringToHash("Walk");
        public static readonly int run = Animator.StringToHash("Run");
        public static readonly int attack = Animator.StringToHash("Attack");
        public static readonly int BeAttack = Animator.StringToHash("BeAttack");
        int groupId { get; }
    }


    public interface IEntityStateMachine<T> : IStateMachine<T> where T : IEntity
    {
        public bool ToIdle(T owner);
        public bool ToWalk(T owner);
        public bool ToRun(T owner);
        public bool ToAttack(T owner);
        public bool ToBeAttack(T owner);
    }
    
    public interface IEntityController : IHittable
    {
        void HandleIdle();
        void HandleWalk(Vector3 viewDir, Vector2 moveInput);
        void HandleRun(Vector3 viewDir, Vector2 moveInput);
        void HandleAttack();
        void HandleBeAttack(int damagePoint, NetworkConnectionToClient sender = null);
    }

    public interface IHittable : IEntity
    {
        bool canBeHit { get; set; }
        void HandleBeAttack(int damagePoint, NetworkConnectionToClient sender = null);
    }

    public interface ITempAnimState
    {
        bool canExit { get; }
        bool over { get; }
    }
}