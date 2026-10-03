using UnityEngine;

namespace PlayerFol.PlStates
{
    public class MoveState : GroundState
    {
        public MoveState(PlayerMovement movement) : base(movement) { }

        public override void Enter()
        {
            
        }
        
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            Movement.SetControlMove();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void Exit()
        {
            
        }
    }
}
