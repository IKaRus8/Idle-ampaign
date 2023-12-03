﻿using Data.Enums;
using GameInfoModels.Interface;
using GameLogic.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace GameLogic.Services
{
    public class SquadStateManager
    {
        private readonly ISquadProvider _playerProvider;

        private BaseState _currentState;

        private List<BaseState> _allStates;

        private Rigidbody _squadRigidbody;
        public SquadStateManager(IEnemyProvider enemyProvider, ISquadProvider playerProvider, Rigidbody rigidbody,
                                    float velocity, float approachRadius, float attackRadius)
        {
            _playerProvider = playerProvider;
            _squadRigidbody = rigidbody;

            _allStates = new List<BaseState>()
            {
                new SquadWalkState(velocity,approachRadius,rigidbody,enemyProvider, this),
                new SquadChaseState(enemyProvider,playerProvider,attackRadius,this),
                new SquadAttackState(this)
            };

            _currentState = _allStates.FirstOrDefault(state => state.GameState == GameState.Walk);
        }
        public void RunState()
        {
            _currentState.RunCurrentState();
        }
        public void SwitchState(GameState gameState)
        {
            var currentState = _allStates.FirstOrDefault(s => s.GameState == gameState);
            _currentState = currentState;
            ChangePlayerBehaviour();
        }
        private void ChangePlayerBehaviour()
        {
            switch (_currentState.GameState)
            {
                case GameState.Walk:
                    _playerProvider.Units[0].PlayerObject.transform.localPosition = Vector3.zero;
                    break;
                case GameState.Chase:
                    _squadRigidbody.velocity = Vector3.forward;
                    break;
                default:
                    break;
            }
        }
    }
}