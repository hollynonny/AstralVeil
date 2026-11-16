using BossFol;
using Managers;
using PlayerFol;
using UnityEngine;

namespace EnemyFol
{
    public abstract class Enemy : MonoBehaviour, IResettable, IDamageable
    {
        private static readonly int IdleHash = Animator.StringToHash("Enemy_Idle");

        public EnemyStateManager StateManager { get; } = new();
        private EnemyState _initialState;

        public BoxCollider2D Collider { get; private set; }
        public SpriteRenderer SpriteRenderer { get; private set; }
        public Animator Animator { get; private set; }
        
        public bool IsDead { get; private set; }

        protected virtual void Start()
        {
            StateManager.Initialize(_initialState);
        }
        
        protected virtual void Awake()
        {
            InitializeComponents();
            
            _initialState = CreateStates();
        }

        protected abstract EnemyState CreateStates();

        protected virtual void InitializeComponents()
        {
            Collider = GetComponent<BoxCollider2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();
        }

        public virtual void TakeDamage(int damage)
        {
            Kill();
        }

        public virtual void Kill()
        {
            IsDead = true;
            Collider.enabled = false;
            SpriteRenderer.enabled = false;
            Animator.enabled = false;
        }

        public void ResetState() => Revive();

        protected virtual void Revive()
        {
            IsDead = false;
            Collider.enabled = true;
            SpriteRenderer.enabled = true;
            Animator.enabled = true;
        }
    }
}
