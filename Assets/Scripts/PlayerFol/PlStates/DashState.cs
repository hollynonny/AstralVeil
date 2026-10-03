using PlayerFol.PlayerDataStructs;
using UnityEngine;

namespace PlayerFol.PlStates
{
    public class DashState : PlayerState
    {
        private ParticleSystem _particleSystem;

        public DashState(PlayerMovement movement, ParticleSystem pS) : base(movement)
        {
            _particleSystem = pS;
        }

        public override void Enter()
        {
            float direction = Movement.PlayerData.MoveInput.x != 0 ? 
                Mathf.Sign(Movement.PlayerData.MoveInput.x) : (Movement.PlayerFlags.IsFacingRight ? 1 : -1);
        
            Movement.PlayerData.Rigidbody.linearVelocity = new Vector2(
                Movement.PlayerParameters.DashForce * direction,
                0
            );

            float angle = Movement.PlayerFlags.IsFacingRight ? 180f : 0f;
            _particleSystem.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            var emission = _particleSystem.emission;
            emission.rateOverTime = 80f;
            _particleSystem.Play();
        }

        public override void LogicUpdate()
        {
            Movement.UpdateDashState();
        }

        public override void PhysicsUpdate()
        {
            
        }

        public override void Exit()
        {
            Movement.PlayerData.Rigidbody.linearVelocity = Vector2.zero;
            Movement.TurnOnGravity();
            
            var emission = _particleSystem.emission;
            emission.rateOverTime = 0f;
            _particleSystem.Stop();
        }
    }
}
