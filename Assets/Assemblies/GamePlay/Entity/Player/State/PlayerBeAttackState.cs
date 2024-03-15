using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PlayerBeAttackState : IState<PlayerController>, ITempAnimState
    {
        public bool canExit => over;
        public bool over { get; private set; }

        public void OnEnter(PlayerController owner)
        {
            over = false;
            owner.canBeHit = false;
            owner.animator.SetTrigger(IEntity.beAttack);
            Debug.Log("PlayerBeAttackState OnEnter");
            Timer.Register(2f, () => { over = true; });
        }

        public void OnUpdate(PlayerController owner)
        {
        }

        public void OnExit(PlayerController owner)
        {
            Debug.Log("PlayerBeAttackState OnExit");
            owner.canBeHit = true;
        }
    }
}