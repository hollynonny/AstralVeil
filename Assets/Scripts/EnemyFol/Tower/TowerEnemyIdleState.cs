using UnityEngine;

namespace EnemyFol.Tower
{
    public class TowerEnemyIdleState : EnemyState<TowerEnemy>
    {
        protected override int AnimationHash => TowerEnemyStates.IdleHash;
        
        public TowerEnemyIdleState(TowerEnemy controller) : base(controller) {}

        public override void Enter()
        {
            Controller.Animator.SetBool(AnimationHash, true);
        }

        public override void LogicUpdate()
        {
            
        }

        public override void PhysicsUpdate()
        {
            
        }

        public override void Exit()
        {
            Controller.Animator.SetBool(AnimationHash, false);
        }
    }
}
