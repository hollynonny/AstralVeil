using UnityEngine;

namespace BossFol.StoneGolem
{
    public sealed class BossHandAttackState : BossAttackState<StoneGolemController>
    {
        protected override int AnimationBoolHash => StoneGolemController.IsHandAttackingHash;
        protected override BossState NextState => Controller.States.Idle;
        
        public BossHandAttackState(StoneGolemController controller) : base(controller)
        { }

        public override void PhysicsUpdate()
        { }
    }
}
