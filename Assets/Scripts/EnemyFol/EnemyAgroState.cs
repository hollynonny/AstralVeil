using UnityEngine;

namespace EnemyFol
{
    public class EnemyAgroState : EnemyState
    {
        private float Speed { get; } = 3.0f;

        public EnemyAgroState(Enemy enemy) : base(enemy)
        { }

        public override void Enter()
        {
            
        }

        public override void LogicUpdate()
        {
            Vector2 nextStep = Vector2.MoveTowards(
                Enemy.transform.position,
                Enemy.Player.transform.position,
                Speed * Time.deltaTime
            );

            Enemy.transform.position = new Vector3(
                nextStep.x, nextStep.y, Enemy.transform.position.z
            );
        }

        public override void PhysicsUpdate()
        {
        }

        public override void Exit()
        {
            
        }
    }
}
