using UnityEngine;

namespace BossFol.CrystalGuardian
{
    public class CrystalGuardianController : BossController
    {
        public static readonly int IdleHash = Animator.StringToHash("Idle");
        public static readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
        public static readonly int ChillHash = Animator.StringToHash("Chill");
        public static readonly int LazerHash = Animator.StringToHash("Lazer");
    
        public CrystalGuardianStates States { get; private set; }

        protected override BossState CreateStates()
        {
            States = new CrystalGuardianStates(this);
            return States.Idle;
        }

        protected override void OnDeath()
        {
            StateManager.ChangeState(States.Death);
        }
    }
}
