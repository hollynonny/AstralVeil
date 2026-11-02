using UnityEngine;

namespace BossFol
{
    public abstract class BossAttackState<TController> : BossState<TController>
        where TController : BossController
    {
        private float _elapsed;
        private float _maxDuration = 5.0f;
        
        protected abstract int AnimationBoolHash { get; }
        protected abstract BossState NextState { get; }

        protected virtual bool FacePlayerOnEnter => true;
        
        public BossAttackState(TController controller) : base(controller)
        { }

        public override void Enter()
        {
            _elapsed = _maxDuration;
            
            Controller.StartAction();
            Animator.SetBool(AnimationBoolHash, true);
            
            if(FacePlayerOnEnter)
                Controller.FacePlayer();
        }

        public override void LogicUpdate()
        {
            _elapsed -= Time.deltaTime;
            if(Controller.ActionFinished || _elapsed <= 0)
                StateManager.ChangeState(NextState);
        }

        public override void Exit()
        {
            Animator.SetBool(AnimationBoolHash, false);
        }
    }
}
