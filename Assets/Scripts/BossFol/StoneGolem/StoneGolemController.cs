using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BossFol.StoneGolem
{
    public enum StoneGolemLimbs
    {
        Head,
        Core,
        LeftHand,
        RightHand,
        LeftLeg,
        RightLeg
    }

    [System.Serializable]
    public struct LimbData
    {
        public StoneGolemLimbs type;
        public Collider2D collider;
    }

    public sealed class StoneGolemController : BossController
    {
        public static readonly int IsHandAttackingHash = Animator.StringToHash("IsHandAttacking");
        public static readonly int IsRightHandAttackingHash = Animator.StringToHash("IsRightHandAttacking");
        public static readonly int IsEarthquakeAttackingHash = Animator.StringToHash("IsEarthquakeAttacking");
        public static readonly int IsChillingHash = Animator.StringToHash("IsChilling");
        public static readonly int IsBulgingHash = Animator.StringToHash("IsBulging");

        private static readonly int LimbCount = Enum.GetValues(typeof(StoneGolemLimbs)).Length;

        [SerializeField] private List<LimbData> limbs = new();
        [SerializeField, Min(0f)] private float handAttackRange = 6f;
        [SerializeField] private CinemachineImpulseSource moveImpulse;
        
        private Collider2D[] _limbColliders;
        private Vector3[] _limbLocalPoses;

        public StoneGolemStates States { get; private set; }

        public float HandAttackRangeSqr => handAttackRange * handAttackRange;
        
        protected override void Awake()
        {
            base.Awake();
            CacheLimbs();
            TurnOffLimbs();
            
            TurnOnLimb(StoneGolemLimbs.Core);
        }

        public override void ResetState()
        {
            if (IsDead) return;
            
            FinishAction();
            
            base.ResetState();
            ApplyPhase(0);

            _lastAttack = null;

            foreach (LimbData limb in limbs)
            {
                int index = (int)limb.type;
                limb.collider.transform.localPosition = _limbLocalPoses[index];
                limb.collider.gameObject.transform.localRotation = Quaternion.identity;
            }
            
            TurnOffLimbs();
            TurnOnLimb(StoneGolemLimbs.Core);
            
            bulga.ResetState();
        }

        public bool RollCombo() => Random.value < CurrentPhase.comboChance;
        
        private BossState _lastAttack;
        public BossState PickAttack()
        {
            if (IsDead) return States.Death;
            
            PhaseSettings s = CurrentPhase;
            
            AttackCombo++;
            
            (BossState state, float weight)[] pool =
            {
                (States.HandAttack, s.handWeight),
                (States.RightHandAttack, s.rightHandWeight),
                (States.EarthquakeAttack, s.earthquakeWeight),
                (States.BulgaAttack, s.bulgaWeight)
            };

            float total = 0.0f;
            foreach (var p in pool)
            {
                if (p.state != _lastAttack) total += p.weight;
            }

            if (total <= 0.0f) return _lastAttack ?? States.HandAttack;

            float roll = Random.value * total;
            foreach (var p in pool)
            {
                if (p.state == _lastAttack) continue;
                roll -= p.weight;
                if (roll <= 0.0f)
                {
                    _lastAttack = p.state;
                    return p.state;
                }
            }

            return States.HandAttack;
        }

        [SerializeField] private SG_Bulga bulga;
        [SerializeField] private Transform rockSpawnPos;
        [SerializeField, Range(0.0f, 1.0f)] private float rockTargetBias = 0.66f;
        [SerializeField] private float rockFlightTime = 2.0f;
        
        public void ThrowRock()
        {
            if (IsDead || bulga == null || !HasPlayer) return;

            bulga.gameObject.SetActive(true);
            bulga.transform.position = rockSpawnPos.position;

            Vector2 from = rockSpawnPos.position;
            Vector2 target = Vector2.Lerp(from, (Vector2)Player.position, rockTargetBias);

            bulga.Launch(target, rockFlightTime);
            ShakeQuake(1.0f);
        }
        
        protected override BossState CreateStates()
        {
            States = new StoneGolemStates(this);
            return States.Idle;
        }

        protected override void OnDeath()
        {
            bulga.ResetState();
            StateManager.ChangeState(States.Death);
        }

        protected override void OnPhaseChanged(int phase)
        {
            ApplyPhase(phase);
        }

        private void ApplyPhase(int phase)
        {
            if (phases == null || phases.Length == 0) return;
            
            PhaseSettings s = phases[Mathf.Min(phase, phases.Length - 1)];
            MoveSpeedMultiplier = s.moveSpeedMultiplier;
            Animator.speed = s.attackSpeedMultiplier;
        }
        
        public void ShakeMove(float force) => moveImpulse?.GenerateImpulseWithForce(force);

        #region Limbs

        public void TurnOnLimbs() => SetAllLimbs(true);
        public void TurnOffLimbs() => SetAllLimbs(false);
        public void TurnOnLimb(StoneGolemLimbs type) => SetLimb(type, true);
        public void TurnOffLimb(StoneGolemLimbs type) => SetLimb(type, false);

        private void CacheLimbs()
        {
            _limbColliders = new Collider2D[LimbCount];
            _limbLocalPoses = new Vector3[LimbCount];

            foreach (LimbData limb in limbs)
            {
                if (limb.collider == null)
                {
                    Debug.LogWarning($"{name}: limb {limb.type} doesn't have collider!", this);
                    continue;
                }

                int index = (int)limb.type;
                if (_limbColliders[index] != null)
                {
                    Debug.LogWarning($"{name}: limb {limb.type} already exists!", this);
                    continue;
                }
                
                _limbColliders[index] = limb.collider;
                _limbLocalPoses[index] = limb.collider.transform.localPosition;
            }
        }

        private void SetLimb(StoneGolemLimbs type, bool enable)
        {
            Collider2D limbCollider = _limbColliders[(int)type];
            if (limbCollider != null)
            {
                limbCollider.enabled = enable;
            }
        }

        private void SetAllLimbs(bool enable)
        {
            for (int i = 0; i < _limbColliders.Length; i++)
            {
                if(_limbColliders[i] != null)
                    _limbColliders[i].enabled = enable;
            }
        }

        #endregion
    }
}
