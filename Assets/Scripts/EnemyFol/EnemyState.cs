using UnityEngine;

namespace EnemyFol
{
    public abstract class EnemyState
    {
        protected Enemy Enemy;

        public EnemyState(Enemy enemy)
        {
            Enemy = enemy;
        }

        public abstract void Enter();
        public abstract void LogicUpdate();
        public abstract void PhysicsUpdate();
        public abstract void Exit();
    }
}
