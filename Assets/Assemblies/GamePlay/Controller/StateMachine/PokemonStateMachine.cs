using System;
using System.Collections.Generic;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonStateMachine : MonoBehaviour, IStateMachine<PokemonController>
    {
        public static readonly int idle = Animator.StringToHash("Idle");
        public static readonly int walk = Animator.StringToHash("Walk");
        public static readonly int run = Animator.StringToHash("Run");
        public static readonly int attack = Animator.StringToHash("Attack");
        public static readonly int BeAttack = Animator.StringToHash("BeAttack");
        
        private Dictionary<Type, IState<PokemonController>> _stateDic;
        public IState<PokemonController> currentState { get; private set; }

        public bool Change<T>(PokemonController owner) where T : IState<PokemonController>
        {
            if (currentState is ITempAnimState { canExit: false })
            {
                return false;
            }

            currentState?.OnExit(owner);
            currentState = _stateDic[typeof(T)];
            currentState.OnEnter(owner);
            return true;
        }

        public void Add<T>(T state) where T : IState<PokemonController>
        {
            _stateDic.Add(typeof(T), state);
        }

        public void Add<T>() where T : IState<PokemonController>, new()
        {
            Add(new T());
        }

        public void OnUpdate(PokemonController owner)
        {
            currentState?.OnUpdate(owner);
        }

        public void Setup(PokemonController owner)
        {
            _stateDic = new Dictionary<Type, IState<PokemonController>>(8);
            Add(GetComponent<PokemonIdleState>());
            Add(GetComponent<PokemonWalkState>());
            Add(GetComponent<PokemonRunState>());
            Add(GetComponent<PokemonAttackState>());
            Add(GetComponent<PokemonBeAttackState>());
            Change<PokemonIdleState>(owner);
        }
    }
}