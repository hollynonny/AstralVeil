namespace EnemyFol.Chaser
{
    public class ChaserIdleState : EnemyState<ChaserEnemy>
    {
        protected override int AnimationHash => ChaserStates.IdleHash;
        
        public ChaserIdleState(ChaserEnemy controller) : base(controller)
        { }

        public override void Enter()
        {
            Controller.Animator.SetBool(AnimationHash, true);
        }

        public override void LogicUpdate()
        {
            if (Controller.AgroDistance >= Controller.DistanceFromPlayerSqr)
            {
                Controller.StateManager.ChangeState(Controller.States.Chase);
                return;
            }
        }

        public override void PhysicsUpdate()
        { }

        public override void Exit()
        {
            Controller.Animator.SetBool(AnimationHash, true);
        }
    }
}
