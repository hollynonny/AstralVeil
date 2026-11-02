using System;
using UnityEngine;

namespace BossFol
{
    public sealed class BossStateManager
    {
        private BossState _pending;
        private bool _isTransitioning;
        
        public BossState CurrentState { get; private set; }
        public BossState PreviousState { get; private set; }

        public event Action<BossState, BossState> StateChanged;
        
        public void Init(BossState state)
        {
            CurrentState = state;
            CurrentState.Enter();
        }
        
        public void ChangeState(BossState state, bool allowReenter = false)
        {
            if (state == null) return;

            if (_isTransitioning)
            {
                _pending = state;
                return;
            }

            if (state == CurrentState && !allowReenter) return;
            
            _isTransitioning = true;
            BossState target = state;

            while (target != null)
            {
                _pending = null;
                CurrentState?.Exit();
                PreviousState = CurrentState;
                CurrentState = target;
                CurrentState.Enter();
                
                StateChanged?.Invoke(PreviousState, CurrentState);
                target = _pending;
            }
            
            _isTransitioning = false;
        }

        public void ForceChangeState(BossState state)
        {
            CurrentState?.Exit();
            PreviousState = CurrentState;
            CurrentState = state;
            CurrentState.Enter();
        }

        public void Tick() => CurrentState?.LogicUpdate();
        public void PhysicsTick() => CurrentState?.PhysicsUpdate();
    }
}
