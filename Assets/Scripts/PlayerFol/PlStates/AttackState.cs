using PlayerFol.PlayerDataStructs;
using UnityEngine;

namespace PlayerFol.PlStates
{
    public class AttackState : PlayerState
    {
        public AttackState(PlayerMovement movement) : base(movement) {}

        public override void Enter()
        {
            Movement.PlayerFlags.OnAirControl = true;
            
            Movement.PlayerAttackData.AttackTimer = Movement.PlayerAttackData.AttackTime;

            Movement.Attack();
        }

        public override void LogicUpdate()
        {
            if (!Movement.AstralSystem.IsAstral)
            {
                Movement.PlayerAttackData.AttackTimer = 0;
                Movement.StateManager.ChangeState(Movement.PlayerStates.FallState);
                return;
            }
            
            if (Movement.PlayerFlags.IsAttacking) return;

            if (Movement.OnEdge())
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.EdgeClimbingState);
                return;
            }
            
            if (Movement.IsGrounded())
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.MoveState);
                return;
            }

            Movement.StateManager.ChangeState(Movement.PlayerStates.FallState);
        }

        public override void PhysicsUpdate()
        {
            Movement.SetControlMove();
            
            Movement.PlayerAttackData.AttackTimer = 
                Mathf.Max(
                    0, 
                    Movement.PlayerAttackData.AttackTimer - Time.fixedDeltaTime
                );
            
            if (Movement.PlayerAttackData.AttackTimer <= 0)
                Movement.PlayerFlags.IsAttacking = false;
        }

        public override void Exit()
        {
            Movement.PlayerFlags.OnAirControl = false;
            
            Movement.PlayerData.HitBox.gameObject.SetActive(false);
            
            Movement.PlayerAttackData.AttackCooldownTimer = 
                Movement.PlayerAttackData.AttackCooldown;
        }
    }
}
