using UnityEngine;

namespace EnemyFol.Chaser
{
    public class ChaserStates
    {
        public static readonly int IdleHash = Animator.StringToHash("Idle");
        public static readonly int ChaseHash = Animator.StringToHash("Chase");
        
        public ChaserIdleState Idle { get; }
        public ChaserChaseState Chase { get; }
        // public ChaserDeathState Death { get; }

        public ChaserStates(ChaserEnemy controller)
        {
            Idle = new(controller);
            Chase = new(controller);
            // Death = new(controller);
        }
    }
}
