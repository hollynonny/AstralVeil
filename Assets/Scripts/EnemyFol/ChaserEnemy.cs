using System;
using Managers;
using UnityEngine;

namespace EnemyFol
{
    public class ChaserEnemy : Enemy
    {
        private void FixedUpdate()
        {
            if (IsDead) return;
            
            StateManager.CurrentState.PhysicsUpdate();
        }

        private void Update()
        {
            if (IsDead) return;
            
            StateManager.CurrentState.LogicUpdate();
        }
    }
}
