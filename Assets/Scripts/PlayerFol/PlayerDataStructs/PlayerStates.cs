using PlayerFol.PlStates;
using UnityEngine;

namespace PlayerFol.PlayerDataStructs
{
    public class PlayerStates
    {
        public MoveState MoveState { get; private set; }
        public JumpState JumpState { get; private set; }
        public FallState FallState { get; private set; }
        public DashState DashState { get; private set; }
        public WallSlidingState WallSlidingState { get; private set; }
        public EdgeClimbingState EdgeClimbingState { get; private set; }
        public AttackState AttackState { get; private set; }

        public PlayerStates(PlayerMovement movement, ParticleSystem pS)
        {
            MoveState = new MoveState(movement);
            JumpState = new JumpState(movement);
            FallState = new FallState(movement);
            DashState = new DashState(movement, pS);
            WallSlidingState = new WallSlidingState(movement);
            EdgeClimbingState = new EdgeClimbingState(movement);
            AttackState = new AttackState(movement);
        }
    }
}
