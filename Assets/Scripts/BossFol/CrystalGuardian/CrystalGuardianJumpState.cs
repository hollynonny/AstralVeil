using UnityEngine;

namespace BossFol.CrystalGuardian
{
    public class CrystalGuardianJumpState : BossState<CrystalGuardianController>
    {
        public CrystalGuardianJumpState(CrystalGuardianController controller) : base(controller)
        {}

        public override void Enter()
        {
            Animator.SetBool(CrystalGuardianController.IsJumpingHash, true);
        }

        public override void LogicUpdate()
        {
            
        }

        public override void PhysicsUpdate()
        {
            
        }
        
        public override void Exit()
        {
            Animator.SetBool(CrystalGuardianController.IsJumpingHash, false);
            
        }
    }
}
