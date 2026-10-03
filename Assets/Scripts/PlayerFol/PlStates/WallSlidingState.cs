using UnityEngine;

namespace PlayerFol.PlStates
{
    public class WallSlidingState : PlayerState
    {
        public WallSlidingState(PlayerMovement movement) :  base(movement) {}

        public override void Enter()
        {
            Movement.PlayerData.Rigidbody.gravityScale = 0.0f;
            Movement.PlayerFlags.SlidingOnWall = true;
        }

        public override void LogicUpdate()
        {
            Movement.UpdateWallTimers();
            
            if (Movement.PlayerFlags.IsDashed)
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
            
            if (Movement.PlayerJumpData.JumpBufferCounter > 0 && Movement.PlayerJumpData.CoyoteTimeCounter > 0)
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.JumpState);
                return;
            }

            if (!Movement.IsTouchingWall())
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.FallState);
                return;
            }
            
            if (Movement.IsGrounded())
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.MoveState);
            }
        }

        public override void PhysicsUpdate()
        {
            Movement.SetControlMove();
        }

        public override void Exit()
        {
            Movement.TurnOnGravity();
            Movement.PlayerFlags.SlidingOnWall = false;
            Movement.PlayerWallData.WallUnstickTimer = Movement.PlayerWallData.WallUnstickTime;
        }
    }
}
