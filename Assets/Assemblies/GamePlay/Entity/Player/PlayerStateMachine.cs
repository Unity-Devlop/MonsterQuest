﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class PlayerStateMachine : MonoBehaviour, IEntityStateMachine<PlayerController>
    {
        private Dictionary<Type, IState<PlayerController>> _stateDic;
        public IState<PlayerController> currentState { get; private set; }

        public bool Change<T>(PlayerController owner) where T : IState<PlayerController>
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

        public void Add<T>(T state) where T : IState<PlayerController>
        {
            _stateDic.Add(typeof(T), state);
        }

        public void Add<T>() where T : IState<PlayerController>, new()
        {
            Add(new T());
        }

        public void OnUpdate(PlayerController owner)
        {
            currentState?.OnUpdate(owner);
        }

        public void Setup(PlayerController owner)
        {
            _stateDic = new Dictionary<Type, IState<PlayerController>>(8);
            Add(new PlayerIdleState());
            Add(new PlayerWalkState());
            Add(new PlayerRunState());
            Add(new PlayerAttackState());
            Add(new PlayerBeAttackState());
            Change<PlayerIdleState>(owner);
        }

        public bool ToIdle(PlayerController owner)
        {
            return Change<PlayerIdleState>(owner);
        }

        public bool ToWalk(PlayerController owner)
        {
            return Change<PlayerWalkState>(owner);
        }

        public bool ToRun(PlayerController owner)
        {
            return Change<PlayerRunState>(owner);
        }

        public bool ToAttack(PlayerController owner)
        {
            return Change<PlayerAttackState>(owner);
        }

        public bool ToBeAttack(PlayerController owner)
        {
            return Change<PlayerBeAttackState>(owner);
        }
    }
}