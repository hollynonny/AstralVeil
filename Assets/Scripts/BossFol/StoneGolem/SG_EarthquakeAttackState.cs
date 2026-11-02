using UnityEngine;

namespace BossFol.StoneGolem
{
    public class SG_EarthquakeAttackState : BossAttackState<StoneGolemController>
    {
        protected override int AnimationBoolHash => StoneGolemController.IsEarthquakeAttackingHash;
        protected override BossState NextState => Controller.States.Idle;

        public SG_EarthquakeAttackState(StoneGolemController controller) : base(controller) {}

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
