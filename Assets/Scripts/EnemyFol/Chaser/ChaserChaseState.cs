namespace EnemyFol.Chaser
{
    public class ChaserChaseState : EnemyState<ChaserEnemy>
    {
        protected override int AnimationHash => ChaserStates.ChaseHash;
        
        public ChaserChaseState(ChaserEnemy controller) : base(controller)
        { }

        public override void Enter()
        {
            Controller.Animator.SetBool(ChaserStates.ChaseHash, true);
        }

        public override void LogicUpdate()
        {
            Controller.FacePlayer();
            Controller.MoveTowardsPlayer();
        }

        public override void PhysicsUpdate()
        {
        }

        public override void Exit()
        {
            Controller.Animator.SetBool(ChaserStates.ChaseHash, false);
        }
    }
}
