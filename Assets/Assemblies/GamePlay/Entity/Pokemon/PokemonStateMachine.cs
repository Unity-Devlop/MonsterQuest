using System;
using System.Collections.Generic;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PokemonStateMachine : MonoBehaviour, IEntityStateMachine<PokemonController>
    {
        private Dictionary<Type, IState<PokemonController>> _stateDic;
        public IState<PokemonController> currentState { get; private set; }

        public bool Change<T>(PokemonController owner) where T : IState<PokemonController>
        {
            if (currentState is ITempAnimState { canExit: false })
            {
                return false;
            }

            if (typeof(T).IsAssignableFrom(typeof(PokemonIdleState)) && currentState is ITempAnimState { over: false })
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
            Add(new PokemonIdleState());
            Add(new PokemonWalkState());
            Add(new PokemonRunState());
            Add(new PokemonAttackState());
            Add(new PokemonBeAttackState());
            Change<PokemonIdleState>(owner);
        }

        public bool ToIdle(PokemonController owner)
        {
            return Change<PokemonIdleState>(owner);
        }

        public bool ToWalk(PokemonController owner)
        {
            return Change<PokemonWalkState>(owner);
        }

        public bool ToRun(PokemonController owner)
        {
            return Change<PokemonRunState>(owner);
        }

        public bool ToAttack(PokemonController owner)
        {
            return Change<PokemonAttackState>(owner);
        }

        public bool ToBeAttack(PokemonController owner)
        {
            return Change<PokemonBeAttackState>(owner);
        }
    }
}