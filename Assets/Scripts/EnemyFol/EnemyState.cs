using UnityEngine;

namespace EnemyFol
{
    public abstract class EnemyState
    {
        public abstract void Enter();
        public abstract void LogicUpdate();
        public abstract void PhysicsUpdate();
        public abstract void Exit();
    }

    public abstract class EnemyState<TController> : EnemyState where TController : Enemy
    {
        protected TController Controller;
        protected abstract int AnimationHash { get; }

        public EnemyState(TController controller)
        {
            Controller = controller;
        }
    }
}
