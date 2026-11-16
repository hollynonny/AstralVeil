using UnityEngine;

namespace EnemyFol
{
    public class EnemyStateManager
    {
        public EnemyState CurrentState;

        public void Initialize(EnemyState state)
        {
            CurrentState = state;
        }

        public void ChangeState(EnemyState state)
        {
            if (state == null) return;
            
            CurrentState.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }
    }
}
