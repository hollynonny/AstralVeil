using UnityEngine;

namespace PlayerFol.PlayerDataStructs
{
    public class PlayerJumpData
    {
        public float CoyoteTime { get; } = 0.1f;
        public float CoyoteTimeCounter { get; set; }
        public float JumpBuffer { get; } = 0.1f;
        public float JumpBufferCounter { get; set; }
        public float MaxJumpHoldTime { get; } = 0.35f;
    }
}
