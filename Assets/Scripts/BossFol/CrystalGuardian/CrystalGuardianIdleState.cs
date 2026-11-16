using UnityEngine;

namespace BossFol.CrystalGuardian
{
    public class CrystalGuardianIdleState : BossState<CrystalGuardianController>
    {
        public CrystalGuardianIdleState(CrystalGuardianController controller) : base(controller)
        {}

        public override void Enter()
        {
            Animator.SetBool(CrystalGuardianController.IdleHash, true);
        }

        public override void LogicUpdate()
        {
            Controller.FacePlayer();
            
            if(Controller.AttackCombo >= Controller.MaxAttackCombo)
                Controller.StateManager.ChangeState(Controller.States.Chill);
        }

        public override void PhysicsUpdate()
        {
            
        }

        public override void Exit()
        {
            Animator.SetBool(CrystalGuardianController.IdleHash, false);
        }
    }
}
