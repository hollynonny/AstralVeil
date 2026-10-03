using UnityEngine;

namespace PlayerFol.PlayerDataStructs
{
    public readonly struct PlayerParameters
    {
        public float Speed => 5.0f;
        public float AirSpeed => 4.0f;
        public float JumpForce => 12.0f;
        public float WallJumpForce => 6.0f;
        public float WallSlideSpeed => 3.0f;
        public float DashForce => 20.0f;
        public float GroundCheckRadius => 0.15f;
        public float DefaultGravityScale => 2.0f;
        public float WallCheckDistance => 0.1f;
        public float ClimbingEdgeSpeed => 5.0f;
        public float ExternalForceDecay => 10.0f;
    }
}
