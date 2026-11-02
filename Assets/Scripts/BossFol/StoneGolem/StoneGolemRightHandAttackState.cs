using UnityEngine;

namespace BossFol.StoneGolem
{
    public class StoneGolemRightHandAttackState : BossAttackState<StoneGolemController>
    {
        protected override int AnimationBoolHash => StoneGolemController.IsRightHandAttackingHash;
        protected override BossState NextState => Controller.States.Idle;
        
        public StoneGolemRightHandAttackState(StoneGolemController controller) : base(controller) {}

        public override void PhysicsUpdate()
        {
        }

        public override void Exit()
        {
            base.Exit();
            Controller.FinishAction();
        }
    }
}
