using Managers;
using UnityEngine;

namespace EnemyFol
{
    public abstract class Enemy : MonoBehaviour, IResettable
    {
        private static readonly int IdleHash = Animator.StringToHash("Enemy_Idle");

        protected BoxCollider2D Collider;
        protected SpriteRenderer SpriteRenderer;
        protected Animator Animator;
        protected Vector3 StartPos;

        public bool IsDead { get; private set; }

        protected virtual void Awake()
        {
            StartPos = transform.position;
            InitializeComponents();
        }
        
        protected virtual void InitializeComponents()
        {
            Collider = GetComponent<BoxCollider2D>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
            Animator = GetComponent<Animator>();
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
            Animator.Play(IdleHash, 0, 0.0f);
        }
    }
}
