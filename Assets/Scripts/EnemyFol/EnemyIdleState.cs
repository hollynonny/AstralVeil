using UnityEngine;

namespace EnemyFol
{
    public class EnemyIdleState : EnemyState
    {
        private readonly float _agroSqr;
        
        public EnemyIdleState(Enemy enemy) : base(enemy)
        {
            _agroSqr = Enemy.AgroDistance * Enemy.AgroDistance;
        }
        
        public override void Enter()
        {
            
        }

        public override void LogicUpdate()
        {
            if (Enemy.SqrDistanceToPlayer < _agroSqr)
            {
                Enemy.StateManager.ChangeState(Enemy.EnemyStates.AgroState);
                return;
            }
        }

        public override void PhysicsUpdate()
        {
            
        }

        public override void Exit()
        {
            
        }
    }
}
