using UnityEngine;

namespace BossFol.StoneGolem
{
    public sealed class StoneGolemDeathState : BossDeathState<StoneGolemController>
    {
        public StoneGolemDeathState(StoneGolemController controller) : base(controller)
        { }

        public override void Enter()
        {
            base.Enter();
            Controller.StopMovement();
            Controller.Animator.SetTrigger(BossController.DeathHash);
            Controller.Animator.SetBool(BossController.IsDeadHash, true);
            Controller.TurnOffLimbs();
        }
    }
}
