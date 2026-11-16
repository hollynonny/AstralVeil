using UnityEngine;

namespace EnemyFol.Tower
{
    public class TowerEnemyStates
    {
        public static readonly int IdleHash = Animator.StringToHash("Idle");
        
        public TowerEnemyIdleState Idle { get; }

        public TowerEnemyStates(TowerEnemy controller)
        {
            Idle = new TowerEnemyIdleState(controller);
        }
    }
}
