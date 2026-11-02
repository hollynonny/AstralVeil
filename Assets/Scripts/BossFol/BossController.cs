using Managers;
using PlayerFol;
using Unity.Cinemachine;
using UnityEngine;

namespace BossFol
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
    public abstract class BossController : MonoBehaviour, IDamageable, IResettable
    {
        public static readonly int MovesTowardsPlayerHash = Animator.StringToHash("MovesTowardsPlayer");
        public static readonly int DeathHash = Animator.StringToHash("Death");
        public static readonly int IsDeadHash = Animator.StringToHash("IsDead");

        private const float FaceDeadZone = 0.05f;

        [SerializeField] private BossConfig config;
        [SerializeField] protected PhaseSettings[] phases;

        [SerializeField] private CinemachineImpulseSource hitImpulse;
        [SerializeField] private CinemachineImpulseSource quakeImpulse;

        [SerializeField] private bool spriteFacesRight;

        private Transform _transform;
        
        private Vector3 _startPos;
        private Vector3 _startScale;
        private int _startFacing;

        private bool _started;
        private bool _restartOnEnable;
        
        private BossState _initialState;

        public BossConfig Config => config;
        public PhaseSettings CurrentPhase => 
            phases.Length == 0 ? default : phases[Mathf.Min(Health.Phase, phases.Length - 1)];
        
        public Transform Player { get; private set; }
        public Animator Animator { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        
        public BossHealth Health { get; private set; }
        public BossStateManager StateManager { get; } = new();

        public float MoveSpeedMultiplier { get; protected set; } = 1.0f;
        
        // 1 = right, -1 = left
        public int FacingDirection { get; private set; }

        public bool ActionFinished { get; private set; }
        
        public bool IsInvincible { get; set; }
        public bool IsDead => Health.IsDead;
        
        public int AttackCombo { get; set; }
        public int MaxAttackCombo => config.MaxAttackCombo;
        public void ResetCombo() => AttackCombo = 0;

        public bool HasPlayer => Player != null;
        
        public float DistanceFromPlayerSqr => HasPlayer ? (Player.position - transform.position).sqrMagnitude : float.PositiveInfinity;

        public int DirectionToPlayer
        {
            get
            {
                if (!HasPlayer) return 0;
                
                float dx = Player.position.x - _transform.position.x;
                if (Mathf.Abs(dx) < FaceDeadZone) return 0;
                return dx > 0f ? 1 : -1;
            }
        }
        
        #region Unity LifeCycle

        protected virtual void Awake()
        {
            _startPos = transform.position;
            _startScale = transform.localScale;
            _transform = transform;
            Animator = GetComponent<Animator>();
            Rigidbody = GetComponent<Rigidbody2D>();

            if (config == null)
            {
                Debug.LogError($"{name}: BOSSCONFIG IS LOST!!!", this);
                enabled = false;
                return;
            }

            Health = new BossHealth(config.MaxHp, config.PhaseThresholds);
            FacingDirection = (_transform.localScale.x > 0f) == spriteFacesRight ? 1 : -1;
            _startFacing = FacingDirection;

            _initialState = CreateStates();
        }

        protected virtual void OnEnable()
        {
            Health.Died += HandleDied;
            Health.PhaseChanged += OnPhaseChanged;

            if (_started && _restartOnEnable)
            {
                _restartOnEnable = false;
                Animator.Rebind();
                Animator.Update(0.0f);
                StateManager.ChangeState(_initialState);
            }
        }

        protected virtual void OnDisable()
        {
            Health.Died -= HandleDied;
            Health.PhaseChanged -= OnPhaseChanged;
        }

        protected virtual void Start()
        {
            Player = RoomManager.Instance.Player;
            StateManager.Init(_initialState);
            _started = true;
        }

        protected virtual void Update()
        {
            StateManager.CurrentState.LogicUpdate();
        }

        protected virtual void FixedUpdate()
        {
            StateManager.CurrentState.PhysicsUpdate();
        }
        
        #endregion
        
        #region Extension Points

        protected abstract BossState CreateStates();

        protected abstract void OnDeath();

        public virtual void ResetState()
        {
            if (IsDead) return;
            
            Rigidbody.linearVelocity = Vector2.zero;
            Rigidbody.angularVelocity = 0f;
            Rigidbody.position = _startPos;
            _transform.position = _startPos;
            
            _transform.localScale = _startScale;
            FacingDirection = _startFacing;
            
            IsInvincible = false;
            ResetCombo();
            
            Health.Reset();
            
            _restartOnEnable = true;
        }
        
        protected virtual void OnPhaseChanged(int phase) {}
        
        #endregion
        
        #region Damage

        public void TakeDamage(int damage)
        {
            if (IsInvincible || IsDead) return;
            Health.Damage(damage);
            
            IsInvincible = true;
        }

        public void ShakeHit(float force) => hitImpulse?.GenerateImpulse(force);
        public void ShakeQuake(float force) => quakeImpulse?.GenerateImpulse(force);

        private void HandleDied()
        {
            IsInvincible = true;
            OnDeath();
        }
        
        #endregion

        #region Facing & Movement
        
        public void FacePlayer() => FaceDirection(DirectionToPlayer);

        public void FaceDirection(int direction)
        {
            if (direction == 0 || direction == FacingDirection) return;
            
            FacingDirection = direction;
            
            Vector3 scale = _transform.localScale;
            float sign = spriteFacesRight ? direction : -direction;
            scale.x = Mathf.Abs(scale.x) * sign;
            _transform.localScale = scale;
        }

        public void MoveHorizontally(float direction)
        {
            Vector2 velocity = Rigidbody.linearVelocity;
            velocity.x = direction * config.MoveSpeed * MoveSpeedMultiplier;
            Rigidbody.linearVelocity = velocity;
        }

        public void MoveTowardsPlayer() => MoveHorizontally(DirectionToPlayer);
        public void StopMovement() => MoveHorizontally(0.0f);
        
        #endregion

        public void FinishAction() => SetActionFinished(true);
        public void StartAction() => SetActionFinished(false);
        private void SetActionFinished(bool isFinished) => ActionFinished = isFinished;
    }
}
