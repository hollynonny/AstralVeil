using UnityEngine;

namespace BossFol.StoneGolem
{
    public class SG_ChillState : BossState<StoneGolemController>
    {
        private float _elapsed;
        private float _maxDuration = 5.0f;
        
        public SG_ChillState(StoneGolemController controller) : base(controller) {}

        public override void Enter()
        {
            _elapsed = _maxDuration;
            
            Controller.StartAction();
            Controller.ResetCombo();
            
            Controller.IsInvincible = false;
            Controller.Animator.SetBool(StoneGolemController.IsChillingHash, true);
        }

        public override void LogicUpdate()
        {
            _elapsed -= Time.deltaTime;
            
            if(Controller.ActionFinished || _elapsed <= 0)
                Controller.StateManager.ChangeState(Controller.States.Idle);
        }

        public override void PhysicsUpdate()
        {
            
        }

        public override void Exit()
        {
            Controller.IsInvincible = true;
            Controller.FinishAction();
            Controller.Animator.SetBool(StoneGolemController.IsChillingHash, false);
        }
    }
}
