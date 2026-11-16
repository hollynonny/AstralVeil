namespace BossFol.CrystalGuardian
{
    public class CrystalGuardianStates 
    {
        public CrystalGuardianIdleState Idle { get; private set; }
        public CrystalGuardianMoveState Move { get; private set; }
        public CrystalGuardianJumpState Jump { get; private set; }
        public CrystalGuardianChillState Chill { get; private set; }
        public CrystalGuardianDeathState Death { get; private set; }

        public CrystalGuardianStates(CrystalGuardianController controller)
        {
            Idle = new CrystalGuardianIdleState(controller);
            Move = new CrystalGuardianMoveState(controller);
            Jump = new CrystalGuardianJumpState(controller);
            Chill = new CrystalGuardianChillState(controller);
            Death = new CrystalGuardianDeathState(controller);
        }
    }
}
