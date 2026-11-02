using UnityEngine;

namespace BossFol
{
    public abstract class BossState
    {
        public abstract void Enter();
        public abstract void LogicUpdate();
        public abstract void PhysicsUpdate();
        public abstract void Exit();
    }

    public abstract class BossState<TController> : BossState where TController : BossController
    {
        protected readonly TController Controller;
        
        protected Animator Animator => Controller.Animator;
        protected BossStateManager StateManager => Controller.StateManager;
        protected BossConfig Config => Controller.Config;

        protected BossState(TController controller)
        {
            Controller = controller;
        }
    }
}
