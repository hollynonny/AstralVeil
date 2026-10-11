using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerFol
{
    public class Player : MonoBehaviour
    {
        
        #region Variables
        
        [SerializeField] private Transform groundCheckPoint;
        [FormerlySerializedAs("hurtBox")] [SerializeField] private HitBoxCollider hitBox;
    
        private PlayerMovement _playerMovement;
        private PlayerAnimation _playerAnimation;
        private Transform _graphicsTransform;

        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;

        #endregion
        
        #region Constructor

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            if(_rb == null) Debug.LogWarning("No Rigidbody2D attached to the Player!");
            
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (_spriteRenderer == null) Debug.LogWarning("No SpriteRenderer attached to the Player!");
            
            ParticleSystem pS = GetComponentInChildren<ParticleSystem>();
            if(pS == null) Debug.LogWarning("No ParticleSystem attached to the Player!");
            pS.Stop();
            
            BoxCollider2D bc = GetComponent<BoxCollider2D>();

            Camera cam = Camera.main;
            
            hitBox.gameObject.SetActive(false);

            _playerAnimation = GetComponentInChildren<PlayerAnimation>();
            _graphicsTransform = _playerAnimation.gameObject.transform;
            
            _playerMovement = new PlayerMovement(_rb, this.transform, groundCheckPoint, bc, cam, pS, hitBox, _graphicsTransform);
        }
        
        #endregion
        
        #region Input Assign
        
        private void OnEnable()
        {
            InputHandler.Instance.OnMove += OnMove;
        
            InputHandler.Instance.OnJumpStarted += OnJumpStarted;
            InputHandler.Instance.OnJumpStopped += OnJumpStopped;

            InputHandler.Instance.OnAttack += OnAttack;
        
            InputHandler.Instance.OnDashStarted += OnDashStarted;
            InputHandler.Instance.OnDashStopped += OnDashStopped;

            InputHandler.Instance.OnModeToggled += OnModeToggle;

            _playerMovement.AstralSystem.AstralStateChanged += _playerMovement.ToggleAstralGravity;
            _playerMovement.PlayerData.HitBox.OnTargetHit += _playerMovement.OnEnemyHit;
        }
    
        private void OnDisable()
        {
            InputHandler.Instance.OnMove -= OnMove;
        
            InputHandler.Instance.OnJumpStarted -= OnJumpStarted;
            InputHandler.Instance.OnJumpStopped -= OnJumpStopped;
            
            InputHandler.Instance.OnAttack -= OnAttack;
        
            InputHandler.Instance.OnDashStarted -= OnDashStarted;
            InputHandler.Instance.OnDashStopped -= OnDashStopped;
            
            InputHandler.Instance.OnModeToggled -= OnModeToggle;
            
            _playerMovement.AstralSystem.AstralStateChanged -= _playerMovement.ToggleAstralGravity;
        }
        
        #endregion
        
        #region Action Assign
    
        #region Movement Actions
        
        private void OnMove(Vector2 input)
        {
            _playerMovement.SetMove(input);
            _playerAnimation.SetMoveAnimation(Mathf.Abs(input.x));
        }
        
        #endregion

        #region Jump Actions
        
        private void OnJumpStarted()
        {
            _playerMovement.OnJumpStarted();
        }

        private void OnJumpStopped()
        {
            _playerMovement.OnJumpStopped();
        }
        
        #endregion
        
        #region Attack Actions

        private void OnAttack()
        {
            _playerMovement.OnAttack();
        }
        
        #endregion
        
        #region Dash Actions

        private void OnDashStarted()
        {
            _playerMovement.OnDashStarted();
        }

        private void OnDashStopped()
        {
            _playerMovement.OnDashStopped();
        }
        
        #endregion
        
        #region Astral Mode Toggle Actions

        private void OnModeToggle()
        {
            _playerMovement.ToggleAstralMode();
        }
        
        #endregion
        
        #endregion
        
        #region Update Methods

        private void FixedUpdate()
        {
            _playerMovement.FixedUpdate();
        }

        private void Update()
        {
            _playerMovement.Update();
            _playerAnimation.SetJumpAnimation(_rb.linearVelocity.y);
            _playerAnimation.SetDashAnimation(_playerMovement.PlayerFlags.IsDashing);
            _playerAnimation.SetWallingAnimation(_playerMovement.PlayerFlags.SlidingOnWall);
            _spriteRenderer.flipX = !_playerMovement.PlayerFlags.IsFacingRight;
        }
        
        #endregion

        private void OnTriggerEnter2D(Collider2D other)
        {
            bool trapped = other.gameObject.layer == LayerManager.TrapLayer;
            bool enemied = other.gameObject.layer == LayerManager.EnemyLayer;
            if (trapped || enemied)
            {
                Die();
            }
        }

        private void Die()
        {
            RoomManager.Instance.Respawn(transform);
            _playerMovement.ResetState();
            // _playerAnimation.ResetState();
        }
    }
}
