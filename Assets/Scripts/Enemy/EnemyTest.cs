using UnityEngine;

namespace Enemy
{
    public class EnemyTest : MonoBehaviour
    {
        private BoxCollider2D _collider;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        
        private const float EnableTime = 1.5f;
        private float _enableTimer;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }
        
        private void FixedUpdate()
        {
            _enableTimer = Mathf.Max(0, _enableTimer - Time.fixedDeltaTime);
            if(_enableTimer <= 0)
                Enable();
        }
        
        public void Disable()
        {
            _collider.enabled = false;
            _spriteRenderer.enabled = false;
            _animator.enabled = false;
            
            _enableTimer = EnableTime;
        }

        private void Enable()
        {
            _collider.enabled = true;
            _spriteRenderer.enabled = true;
            _animator.enabled = true;
        }
    }
}
