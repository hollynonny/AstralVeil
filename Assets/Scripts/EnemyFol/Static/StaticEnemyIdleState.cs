namespace EnemyFol.Static
{
    public class StaticEnemyIdleState : EnemyState<StaticEnemy>
    {
        protected override int AnimationHash => StaticEnemyStates.IdleHash;
        
        public StaticEnemyIdleState(StaticEnemy controller) : base(controller)
        { }

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
