using UnityEngine;

namespace BossFol
{
    public class BossDeathState<TController> : BossState<TController> 
        where TController : BossController
    {
        public BossDeathState(TController controller) : base(controller) {}

        public override void Enter()
        {
            Controller.StopMovement();
            Animator.SetTrigger(BossController.DeathHash);
        }

        public override void LogicUpdate()
        {
        }

        public override void PhysicsUpdate()
        {
        }

        public override void Exit()
        {
        }
    }
}
