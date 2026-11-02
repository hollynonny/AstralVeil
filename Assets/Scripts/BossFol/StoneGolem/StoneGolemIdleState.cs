using UnityEngine;

namespace BossFol.StoneGolem
{
    public sealed class StoneGolemIdleState : BossState<StoneGolemController>
    {
        public StoneGolemIdleState(StoneGolemController controller) : base(controller) {}

        public override void Enter()
        {
            Animator.SetBool(BossController.MovesTowardsPlayerHash, true);
        }

        public override void LogicUpdate()
        {
            Controller.FacePlayer();
            
            if(Controller.AttackCombo >= Controller.MaxAttackCombo)
                StateManager.ChangeState(Controller.States.Chill);
            else if(Controller.DistanceFromPlayerSqr <= Controller.HandAttackRangeSqr)
                StateManager.ChangeState(Controller.PickAttack());
        }

        public override void PhysicsUpdate()
        {
            Controller.MoveTowardsPlayer();
        }

        public override void Exit()
        {
            Animator.SetBool(BossController.MovesTowardsPlayerHash, false);
            Controller.StopMovement();
        }
    }
}
