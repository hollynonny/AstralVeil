namespace PlayerFol.PlStates
{
    public class GroundState : PlayerState
    {
        public GroundState(PlayerMovement movement) : base(movement) { }
        
        public override void Enter() { }

        public override void LogicUpdate()
        {
            if (Movement.AstralSystem.IsAstral && Movement.PlayerFlags.IsAttacking)
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.AttackState);
                return;
            }
            
            if (Movement.PlayerFlags.IsDashed && !Movement.AstralSystem.IsAstral)
            {
                Movement.TurnDashedOff();
                Movement.StateManager.ChangeState(Movement.PlayerStates.DashState);
                return;
            }
            
            if (Movement.PlayerJumpData.JumpBufferCounter > 0 && Movement.PlayerJumpData.CoyoteTimeCounter > 0)
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.JumpState);
                return;
            }

            if (!Movement.IsGrounded() && Movement.PlayerData.Rigidbody.linearVelocity.y < 0)
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.FallState);
            }
        }

        public override void PhysicsUpdate()
        {
            Movement.UpdateGroundedTimers();
        }

        public override void Exit() { }
    }
}
