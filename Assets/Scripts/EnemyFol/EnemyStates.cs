using UnityEngine;

namespace EnemyFol
{
    public class EnemyStates
    {
        public EnemyIdleState IdleState { get; private set; }
        public EnemyAgroState AgroState { get; private set; }

        public EnemyStates(Enemy enemy)
        {
            IdleState = new EnemyIdleState(enemy);
            AgroState = new EnemyAgroState(enemy);
        }
    }
}
