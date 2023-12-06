﻿using Data.Enums;
using GameInfoModels.Interface;
using GameLogic.Interfaces;
using Models;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.State
{
    public class SquadStateManager
    {
        private readonly ISquadUnitsProvider _squadUnitsProvider;
        private readonly IEnemyProvider _enemyProvider;

        private readonly float _squadChaseRadius;
        private readonly float _squadAttackRadius;

        private BaseState _currentState;

        private Dictionary<GameState, BaseState> _allStates;

        private Rigidbody _squadRigidbody;
        public SquadStateManager(IEnemyProvider enemyProvider, ISquadUnitsProvider squadProvider, Rigidbody rigidbody,
                                    float velocity, float squadChaseRadius, float unitAttackRadius, float squadAttackRadius)
        {
            _enemyProvider = enemyProvider;
            _squadChaseRadius = squadChaseRadius;
            _squadAttackRadius = squadAttackRadius;
            _squadUnitsProvider = squadProvider;
            _squadRigidbody = rigidbody;

            _allStates = new Dictionary<GameState, BaseState>()
            {
                { GameState.Walk, new SquadWalkState(velocity,rigidbody) },
                { GameState.Chase, new SquadChaseState(velocity,rigidbody) },
                { GameState.Attack, new SquadAttackState(enemyProvider,squadProvider,unitAttackRadius,squadAttackRadius) }
            };

            _currentState = _allStates[GameState.Walk];
        }
        public void RunState()
        {
            CheckEnemy();
            _currentState.RunCurrentState();
        }
        private void CheckEnemy()
        {
            if (!_enemyProvider.IsEnemyNotExist)
            {
                var enemyContainerPosition = _enemyProvider.Enemies[0].EnemyPosition;
                var distance = Vector3.Distance(enemyContainerPosition, _squadRigidbody.position);
                switch (_currentState.GameState)
                {
                    case GameState.Walk:
                        CheckEnemyForWalkState(distance);
                        break;
                    case GameState.Chase:
                        CheckEnemyForChaseState(distance);
                        break;
                    case GameState.Attack:
                        CheckEnemyForAttackState(distance);
                        break;
                    default:
                        break;
                }
                return;
            }
            SwitchState(GameState.Walk);
        }
        private void CheckEnemyForWalkState(float distance)
        {
            if (distance > _squadChaseRadius)
            {
                return;
            }
            SwitchState(GameState.Chase);
        }
        private void CheckEnemyForChaseState(float distance)
        {
            if (distance < _squadAttackRadius)
            {
                SwitchState(GameState.Attack);
                return;
            }
            if(distance< _squadChaseRadius)
            {
                return;
            }
            SwitchState(GameState.Walk);
        }
        private void CheckEnemyForAttackState(float distance)
        {
            if (distance < _squadAttackRadius)
            {
                return;
            }
            foreach (var unit in _squadUnitsProvider.Units)
            {
                unit.Agent.isStopped = true;
            }
            SwitchState(GameState.Walk);
        }
        public void SwitchState(GameState gameState)
        {
            if (_currentState.GameState != gameState)
            {
                _currentState = _allStates[gameState];
                ChangePlayerBehaviour();
            }
        }
        private void ChangePlayerBehaviour()
        {
            switch (_currentState.GameState)
            {
                case GameState.Walk:
                    ChangeStateInWalk();
                    break;
                case GameState.Attack:
                    ChangeStateInAttack();
                    break;
                default:
                    break;
            }
            Debug.Log(_currentState.GameState);
        }
        private void ChangeStateInWalk()
        {
            foreach (var unit in _squadUnitsProvider.Units)
            {
                unit.UnitObject.transform.localPosition = Vector3.zero;
                unit.UnitObject.transform.localRotation = Quaternion.identity;
            }

        }
        private void ChangeStateInAttack()
        {
            _squadRigidbody.velocity = Vector3.forward;
        }
    }
}
