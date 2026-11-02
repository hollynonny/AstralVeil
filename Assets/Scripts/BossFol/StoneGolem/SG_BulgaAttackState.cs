using UnityEngine;

namespace BossFol.StoneGolem
{
    public class SG_BulgaAttackState : BossAttackState<StoneGolemController>
    {
        protected override int AnimationBoolHash => StoneGolemController.IsBulgingHash;
        protected override BossState NextState =>
            Controller.RollCombo() ? Controller.States.EarthquakeAttack : Controller.States.Idle;

        public SG_BulgaAttackState(StoneGolemController controller) : base(controller) {}

        public override void PhysicsUpdate()
        { }

        public override void Exit()
        {
            base.Exit();
            Controller.FinishAction();
        }
    }
}
