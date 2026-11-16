using UnityEngine;

namespace EnemyFol.Static
{
    public class StaticEnemyStates 
    {
        public static readonly int IdleHash = Animator.StringToHash("Idle");
        
        public StaticEnemyIdleState Idle { get; }
        
        public StaticEnemyStates(StaticEnemy controller)
        {
            Idle = new StaticEnemyIdleState(controller);
        }
    }
}
