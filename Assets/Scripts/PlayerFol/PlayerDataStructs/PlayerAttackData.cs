using UnityEngine;

namespace PlayerFol.PlayerDataStructs
{
    public class PlayerAttackData
    {
        public float AttackTime { get; } = 0.2f;
        public float AttackTimer { get; set; }
        public float AttackCooldown { get; } = 0.2f;
        public float AttackCooldownTimer { get; set; }
        public int AttackSector { get; set; }
        public Vector2Int AttackDirection { get; set; }
        public int PogoJumpForce { get; } = 8;
        public float PogoJumpTime { get; } = 0.1f;
        public float PogoJumpTimer { get; set; }

        public float AttackDirectionDeadZone { get; } = 0.1f;
            
        public readonly Vector2[] HurtBoxPoses = new[]
        {
            new Vector2(0.7f, 0.0f),
            new Vector2(0.7f, 0.35f),
            new Vector2(0.0f, 0.7f),
            new Vector2(-0.7f, 0.35f),
            new Vector2(-0.7f, 0.0f),
            new Vector2(-0.7f, -0.35f),
            new Vector2(0.0f, -0.7f),
            new Vector2(0.7f, -0.35f),
        };
        public readonly float[] HurtBoxAngles = new[]
        {
            0.0f, 135.0f, 0.0f, 225.0f, 0.0f, 315.0f, 0.0f, 45.0f
        };
    }
}
