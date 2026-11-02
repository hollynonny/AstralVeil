using System;
using System.Collections.Generic;
using UnityEngine;

namespace BossFol
{
    public sealed class BossHealth
    {
        private readonly float[] _phaseThresholds;
        
        public int Max { get; }
        public int Current { get; private set; }
        public int Phase { get; private set; }
        public float Ratio => (float)Current / Max;
        public bool IsDead => Current <= 0;

        public event Action<int> Damaged;
        public event Action<int> PhaseChanged;
        public event Action Died;

        public BossHealth(int maxHp, IReadOnlyList<float> phaseThresholds)
        {
            Max = maxHp;
            Current = maxHp;
            
            _phaseThresholds = new float[phaseThresholds.Count];
            for (int i = 0; i < _phaseThresholds.Length; i++)
                _phaseThresholds[i] = phaseThresholds[i];
            
            Array.Sort(_phaseThresholds);
            Array.Reverse(_phaseThresholds);
        }

        public void Reset()
        {
            Current = Max;
            Phase = 0;
        }

        public void Damage(int amount)
        {
            if (IsDead || amount <= 0) return;
            
            Current = Math.Max(0, Current - amount);
            Damaged?.Invoke(amount);
            
            Debug.Log($"HP changed to {Current}");

            if (IsDead)
            {
                Died?.Invoke();
                return;
            }

            UpdatePhase();
        }

        private void UpdatePhase()
        {
            int newPhase = Phase;
            float ratio = Ratio;

            while (newPhase < _phaseThresholds.Length && ratio <= _phaseThresholds[newPhase])
                newPhase++;

            if (newPhase == Phase) return;
            
            Phase = newPhase;
            Debug.Log($"Phase changed to {newPhase}");
            PhaseChanged?.Invoke(newPhase);
        }
    }
}
