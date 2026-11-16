using UnityEngine;

namespace BossFol.CrystalGuardian
{
    public class CrystalGuardianChillState : BossState<CrystalGuardianController>
    {
        private const float Timer = 1.0f;
        private float _timer;
        
        public CrystalGuardianChillState(CrystalGuardianController controller) : base(controller)
        {}

        public override void Enter()
        {
            Animator.SetBool(CrystalGuardianController.ChillHash, true);
            _timer = Timer;
        }

        public override void LogicUpdate()
        {
            _timer -= Time.deltaTime;
            if(_timer <= 0)
                Controller.StateManager.ChangeState(Controller.States.Idle);
        }

        public override void PhysicsUpdate()
        {
            
        }

        public override void Exit()
        {
            Animator.SetBool(CrystalGuardianController.ChillHash, true);
            
        }
    }
}
