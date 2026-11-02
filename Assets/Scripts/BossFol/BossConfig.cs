using System.Collections.Generic;
using UnityEngine;

namespace BossFol
{
    [CreateAssetMenu(fileName = "BossConfig", menuName = "BossFol/BossConfig")]
    public class BossConfig : ScriptableObject
    {
        [Min(1)] [SerializeField] private int maxHp = 10;
        [Min(0f)] [SerializeField] private float moveSpeed = 1f;
        [Min(1)] [SerializeField] private int maxAttackCombo = 3;

        [Tooltip("Pomer zbyvajiciho HP (0-1), pri kterem zacina nova faze. Napr. 0.66 a 0.33 = 3 faze")]
        [SerializeField] private float[] phaseThresholds = { 0.66f };

        public int MaxHp => maxHp;
        public float MoveSpeed => moveSpeed;
        public int MaxAttackCombo => maxAttackCombo;
        public IReadOnlyList<float> PhaseThresholds => phaseThresholds;
    }
}
