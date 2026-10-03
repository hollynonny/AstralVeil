namespace PlayerFol.PlStates
{
    public class FallState : PlayerState
    {
        public FallState(PlayerMovement movement) : base(movement) { }

        public override void Enter()
        {
            Movement.PlayerFlags.OnAirControl = true;
        }

        public override void LogicUpdate()
        {
            if (Movement.AstralSystem.IsAstral && Movement.PlayerFlags.IsAttacking)
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.AttackState);
                return;
            }
            
            if (!Movement.AstralSystem.IsAstral && Movement.PlayerFlags.IsDashed)
            {
                Movement.TurnDashedOff();
                Movement.StateManager.ChangeState(Movement.PlayerStates.DashState);
                return;
            }
            
            if (Movement.OnEdge())
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.EdgeClimbingState);
                return;
            }

            if (!Movement.AstralSystem.IsAstral && Movement.IsTouchingWall())
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.WallSlidingState);
                return;
            }

            if (Movement.IsGrounded())
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.MoveState);
            }
        }

        public override void PhysicsUpdate()
        {
            Movement.UpdateFallTimers();   
            Movement.SetControlMove();
        }


        public override void Exit()
        {
            Movement.PlayerFlags.OnAirControl = false;
        }
    }
}
