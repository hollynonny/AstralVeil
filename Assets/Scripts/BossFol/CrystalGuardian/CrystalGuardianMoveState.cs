using UnityEngine;

namespace BossFol.CrystalGuardian
{
    public class CrystalGuardianMoveState : BossState<CrystalGuardianController>
    {
        public CrystalGuardianMoveState(CrystalGuardianController controller) : base(controller)
        {
            
        }

        public override void Enter()
        {
            Animator.SetBool(BossController.MovesTowardsPlayerHash, true);
        }

        public override void LogicUpdate()
        {
            
        }

        public override void PhysicsUpdate()
        {
            
        }

        public override void Exit()
        {
            Animator.SetBool(BossController.MovesTowardsPlayerHash, true);
            
        }
    }
}
