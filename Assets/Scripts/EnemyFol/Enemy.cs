using BossFol;
using Managers;
using PlayerFol;
using UnityEngine;

namespace EnemyFol
{
    public abstract class Enemy : MonoBehaviour, IResettable, IDamageable
    {
        private static readonly int IdleHash = Animator.StringToHash("Enemy_Idle");

        public EnemyStateManager StateManager { get; protected set; }
        public EnemyStates EnemyStates { get; protected set; }

        protected BoxCollider2D Collider;
        protected SpriteRenderer SpriteRenderer;
        protected Animator Animator;
        protected Vector3 StartPos;
        
        public Transform Player { get; private set; }

        public bool IsDead { get; private set; }
        public float AgroDistance { get; } = 7.5f;
        public float SqrDistanceToPlayer => ((Vector2)(Player.transform.position - transform.position)).sqrMagnitude;

        protected virtual void Awake()
        {
            StartPos = transform.position;
            
            InitializeComponents();

            EnemyStates = new EnemyStates(this);

            StateManager = new EnemyStateManager();
            StateManager.Initialize(EnemyStates.IdleState);
        }

        protected virtual void Start()
        {
            Player = RoomManager.Instance.Player;
        }
        
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
            transform.position = StartPos;
            Animator.enabled = true;
            
            StateManager.ChangeState(EnemyStates.IdleState);
        }
    }
}
