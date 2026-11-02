using UnityEngine;

namespace BossFol.StoneGolem
{
    public sealed class StoneGolemStates
    {
        public StoneGolemIdleState Idle { get; }
        public BossHandAttackState HandAttack { get; }
        public StoneGolemRightHandAttackState RightHandAttack { get; }
        public SG_EarthquakeAttackState EarthquakeAttack { get; }
        public SG_BulgaAttackState BulgaAttack { get; }
        public SG_ChillState Chill { get; }
        public StoneGolemDeathState Death { get; }

        public StoneGolemStates(StoneGolemController controller)
        {
            Idle = new StoneGolemIdleState(controller);
            HandAttack = new BossHandAttackState(controller);
            Death = new StoneGolemDeathState(controller);
            RightHandAttack = new StoneGolemRightHandAttackState(controller);
            EarthquakeAttack = new SG_EarthquakeAttackState(controller);
            Chill = new SG_ChillState(controller);
            BulgaAttack = new SG_BulgaAttackState(controller);
        }
    }
}
