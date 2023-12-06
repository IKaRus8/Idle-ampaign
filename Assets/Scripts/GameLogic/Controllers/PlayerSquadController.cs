using GameInfoModels.Interface;
using GameLogic.Interfaces;
using GameLogic.State;
using UnityEngine;
using Zenject;

namespace GameLogic.Controllers
{
    public class PlayerSquadController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody _rigidbody;
        [SerializeField]
        private float _squadChaseRadius;
        [SerializeField]
        private float _squadAttackRadius;
        [SerializeField]
        private float _unitAttackRadius;

        private IEnemyProvider _enemyProvider;
        private ISquadUnitsProvider _squadUnitsProvider;
        private SquadUnitsStateManager _squadUnitsStateManager;
        public float Velocity => 20f;

        [Inject]
        void Construct(IEnemyProvider enemyProvider, ISquadUnitsProvider squadUnitsProvider)
        {
            _enemyProvider = enemyProvider;
            _squadUnitsProvider = squadUnitsProvider;
        }
        private void Start()
        {
            _squadUnitsStateManager = new SquadUnitsStateManager(_enemyProvider, _squadUnitsProvider, _rigidbody, Velocity, _squadChaseRadius, _squadAttackRadius, _unitAttackRadius);
        }
        private void FixedUpdate()
        {
            _squadUnitsStateManager.RunState();
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _unitAttackRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _squadAttackRadius);
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, _squadChaseRadius);
        }
    }
}