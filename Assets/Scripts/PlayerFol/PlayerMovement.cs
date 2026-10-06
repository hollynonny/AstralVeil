using Enemy;
using Managers;
using PlayerFol.PlayerDataStructs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerFol
{
    public class PlayerMovement
    {
        #region Variables
        
        public PlayerData PlayerData { get; private set; }
        public PlayerStates PlayerStates { get; private set; }
        public PlayerFlags PlayerFlags { get; private set; }

        public PlayerStateManager StateManager { get; private set; }
        public AstralSystem AstralSystem { get; private set; }
        
        public PlayerParameters PlayerParameters { get; } = new();
        public PlayerJumpData PlayerJumpData { get; } = new();
        public PlayerDashData PlayerDashData { get; } = new();
        public PlayerWallData PlayerWallData { get; } = new();
        public PlayerEdgeClimbData PlayerEdgeClimbData { get; } = new();
        public PlayerAttackData PlayerAttackData { get; } = new();
        
        #endregion

        #region Constructor
        
        public PlayerMovement(Rigidbody2D rb, Transform playerTransform, Transform groundCheckPoint,
            BoxCollider2D bc, Camera camera, ParticleSystem pS, HitBoxCollider hitBox)
        {
            PlayerData = new PlayerData(playerTransform, groundCheckPoint, rb, bc, camera, hitBox);
            PlayerStates = new PlayerStates(this, pS);
            PlayerFlags = new PlayerFlags();
            
            StateManager = new PlayerStateManager();
            StateManager.Initialize(PlayerStates.MoveState);
            
            AstralSystem = new AstralSystem();
        }
        
        #endregion

        #region Update Methods
        
        public void Update()
        {
            StateManager.CurrentState.LogicUpdate();
        }

        public void FixedUpdate()
        {
            PlayerJumpData.JumpBufferCounter = Mathf.Max(0, PlayerJumpData.JumpBufferCounter - Time.fixedDeltaTime);
            PlayerWallData.WallUnstickTimer = Mathf.Max(0, PlayerWallData.WallUnstickTimer - Time.fixedDeltaTime);
            PlayerAttackData.AttackCooldownTimer = Mathf.Max(0, PlayerAttackData.AttackCooldownTimer - Time.fixedDeltaTime);
            
            if(!PlayerFlags.CanDash)
                PlayerDashData.DashCooldownCounter = Mathf.Max(0, PlayerDashData.DashCooldownCounter - Time.fixedDeltaTime);

            if (PlayerDashData.DashCooldownCounter <= 0 && PlayerFlags.OnObjectDash)
            {
                PlayerFlags.OnObjectDash = false;
                SetCanDash(true);
            }

            PlayerAttackData.PogoJumpTimer = Mathf.Max(0, PlayerAttackData.PogoJumpTimer - Time.fixedDeltaTime);
            
            StateManager.CurrentState.PhysicsUpdate();
        }
        
        #endregion
        
        #region Jump Methods
        
        public void ApplyJumpForce()
        {
            PlayerJumpData.JumpBufferCounter = 0.0f;
            PlayerJumpData.CoyoteTimeCounter = 0.0f;

            if (IsTouchingWall() || PlayerWallData.WallUnstickTimer > 0)
            {
                float jumpOutDirection = PlayerFlags.IsFacingRight ? -1.0f : 1.0f;
                float direction = (PlayerParameters.WallJumpForce * jumpOutDirection) 
                            + (PlayerData.MoveInput.x * (PlayerParameters.Speed * 0.5f));
                PlayerData.ExternalForce += new Vector2(direction, 0);
            }

            float finalY = Mathf.Min(PlayerParameters.MaxAirFlySpeed, PlayerParameters.JumpForce * AstralSystem.JumpMultiplier);
            PlayerData.Rigidbody.linearVelocity = new Vector2(
                PlayerData.Rigidbody.linearVelocity.x * AstralSystem.MovementMultiplier,
                finalY
            );
        }

        public void CutJumpHeight()
        {
            PlayerData.Rigidbody.linearVelocity = new Vector2(
                PlayerData.Rigidbody.linearVelocity.x,
                PlayerData.Rigidbody.linearVelocity.y * 0.3f
            );
        }

        public void OnJumpStarted()
        {
            PlayerJumpData.JumpBufferCounter = PlayerJumpData.JumpBuffer;
            PlayerJumpData.IsJumpHeld = true;
        }

        public void OnJumpStopped()
        {
            PlayerJumpData.IsJumpHeld = false;
        }
        
        #endregion

        #region Timers
        
        public void UpdateGroundedTimers()
        {
            PlayerJumpData.CoyoteTimeCounter = PlayerJumpData.CoyoteTime;

            if (PlayerDashData.DashCooldownCounter <= 0)
                SetCanDash(true);
        }

        public void UpdateFallTimers()
        { 
            PlayerJumpData.CoyoteTimeCounter = Mathf.Max(0, PlayerJumpData.CoyoteTimeCounter - Time.fixedDeltaTime);
        }

        public void UpdateWallTimers()
        {
            PlayerJumpData.CoyoteTimeCounter = PlayerJumpData.CoyoteTime;
            
            if(PlayerDashData.DashCooldownCounter <= 0)
                SetCanDash(true);
        }

        public void StartEdgeClimbingState()
        {
            PlayerFlags.ClimbingOnEdge = true;
        }

        public void UpdateEdgeClimbingState(Vector2 targetPos, Vector2 currentPos)
        {
            float distance = Vector2.Distance(targetPos, currentPos);
            if (distance < 0.01f)
            {
                PlayerFlags.ClimbingOnEdge = false;
            }
        } 
        
        #endregion
        
        #region Attack Methods

        public void OnAttack()
        {
            if (!AstralSystem.IsAstral || PlayerAttackData.AttackCooldownTimer > 0) return;
            PlayerFlags.IsAttacking = true;
        }

        public Vector2Int Get8DirectionFromMouse()
        {
            Vector3 playerPos = 
                PlayerData.Camera.WorldToScreenPoint(PlayerData.PlayerTransform.position);
            
            Vector3 mousePos = Mouse.current.position.ReadValue();

            Vector2 direction = (mousePos - playerPos).normalized;
            
            if(direction.sqrMagnitude < PlayerAttackData.AttackDirectionDeadZone)
                return PlayerFlags.IsFacingRight ? Vector2Int.right : Vector2Int.left;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            return AngleTo8Direction(angle);
        }

        public void Attack()
        {
            PlayerAttackData.AttackDirection = Get8DirectionFromMouse();
            
            Vector2 hurtBoxPos = PlayerAttackData.HurtBoxPoses[PlayerAttackData.AttackSector];
            float hurtBoxAngle = PlayerAttackData.HurtBoxAngles[PlayerAttackData.AttackSector];
            
            PlayerData.HitBox.gameObject.transform.localPosition = hurtBoxPos;
            PlayerData.HitBox.gameObject.transform.localRotation = Quaternion.Euler(0, 0, hurtBoxAngle);
            
            PlayerData.HitBox.gameObject.SetActive(true);
        }

        public void OnEnemyHit(Collider2D collider)
        {
            if (collider.gameObject.layer == LayerManager.EnemyLayer)
            {
                EnemyTest enemyScript = collider.gameObject.GetComponent<EnemyTest>();
                enemyScript.Disable();
                
                PogoJump();
            }
        }

        private void PogoJump()
        {
            PlayerAttackData.PogoJumpTimer = PlayerAttackData.PogoJumpTime;
            
            Vector2 dir = ((Vector2)PlayerAttackData.AttackDirection).normalized;
            PlayerData.ExternalForce += -dir * PlayerAttackData.PogoJumpForce;

            SetCanDash(true);
        }

        private Vector2Int AngleTo8Direction(float angle)
        {
            angle += 22.5f;

            if (angle < 0) angle += 360.0f;
            
            int sector = Mathf.FloorToInt(angle / 45.0f) % 8;
            PlayerAttackData.AttackSector = sector;

            return sector switch
            {
                0 => new Vector2Int(1, 0),
                1 => new Vector2Int(1, 1),
                2 => new Vector2Int(0, 1),
                3 => new Vector2Int(-1, 1),
                4 => new Vector2Int(-1, 0),
                5 => new Vector2Int(-1, -1),
                6 => new Vector2Int(0, -1),
                7 => new Vector2Int(1, -1),
                _ => Vector2Int.right
            };
        }
        
        #endregion

        #region Movement Methods

        public void SetMove(Vector2 input)
        {
            PlayerData.MoveInput = input;
            
            if (input.x == 0) return;
            PlayerFlags.IsFacingRight = input.x > 0 || !(input.x < 0);
        }

        public void SetControlMove()
        {
            float moveMultiplier = PlayerFlags.OnAirControl ? PlayerParameters.AirSpeed : PlayerParameters.Speed;
            float targetControlX;
            if (PlayerWallData.WallUnstickTimer <= 0)
                targetControlX = PlayerData.MoveInput.x * moveMultiplier * AstralSystem.MovementMultiplier;
            else
                targetControlX = 0;
            
            PlayerData.ExternalForce = Vector2.MoveTowards(
                PlayerData.ExternalForce,
                Vector2.zero,
                PlayerParameters.ExternalForceDecay * Time.fixedDeltaTime
            );

            float finalX = targetControlX + PlayerData.ExternalForce.x;
            float finalY = 0.0f;

            if (PlayerFlags.SlidingOnWall && PlayerWallData.WallUnstickTimer <= 0)
            {
                finalY = -PlayerParameters.WallSlideSpeed;
            }
            else
            {
                if (Mathf.Abs(PlayerData.ExternalForce.y) > 0.01f)
                {
                    finalY = PlayerData.ExternalForce.y;
                    PlayerData.ExternalForce = new Vector2(PlayerData.ExternalForce.x, 0);
                }
                else
                    finalY = PlayerData.Rigidbody.linearVelocity.y;
            }
            
            finalY = Mathf.Min(PlayerParameters.MaxAirFlySpeed, finalY);
            PlayerData.Rigidbody.linearVelocity = new Vector2(finalX, finalY);
        }
        
        #endregion

        #region Dash Methods
        
        public void OnDashStarted()
        {
            if (AstralSystem.IsAstral) return;
            if (!PlayerFlags.CanDash || PlayerFlags.IsDashing) return;

            PlayerFlags.IsDashHeld = true;
            
            PlayerFlags.IsDashed = true;
            
            PlayerFlags.IsDashing = true;
            SetCanDash(false);
            PlayerDashData.DashTimerCounter = PlayerDashData.DashTimer; 
            PlayerData.Rigidbody.gravityScale = 0;
        }

        public void OnDashStopped()
        {
            PlayerFlags.IsDashHeld = false;
        }

        public void TurnDashedOff()
        {
            PlayerFlags.IsDashed = false;
        }

        public void UpdateDashState()
        {
            PlayerDashData.DashTimerCounter -= Time.deltaTime;
            
            if (PlayerDashData.DashTimerCounter <= 0 || !PlayerFlags.IsDashHeld)
            {
                if (IsGrounded() || IsTouchingWall())
                    PlayerFlags.OnObjectDash = true;
                
                PlayerFlags.IsDashing = false;
                PlayerDashData.DashCooldownCounter = PlayerDashData.DashCooldown;
                StateManager.ChangeState(PlayerStates.FallState);
            }
        }
        
        private void SetCanDash(bool canDash)
        {
            PlayerFlags.CanDash = canDash;
        }
        
        #endregion

        #region Astral Methods

        public void ToggleAstralMode()
        {
            if(!AstralSystem.IsAstral)
                AstralSystem.EnterAstralState();
            else
                AstralSystem.ExitAstralState();
        }
        
        #endregion
        
        #region Gravity Methods
        
        public void TurnOnGravity()
        {
            PlayerData.Rigidbody.gravityScale = 
                PlayerParameters.DefaultGravityScale 
                * AstralSystem.GravityMultiplier;
        }

        public void ToggleAstralGravity()
        {
            PlayerData.Rigidbody.gravityScale = 
                PlayerParameters.DefaultGravityScale 
                * AstralSystem.GravityMultiplier;
        }
        
        #endregion

        #region Check Methods
        
        public bool IsGrounded()
        {
            return Physics2D.OverlapCircle(
                PlayerData.GroundCheckPoint.position, 
                PlayerParameters.GroundCheckRadius, 
                LayerManager.GroundLayerMask
            );
        }

        public bool IsTouchingWall()
        {
            if (PlayerWallData.WallUnstickTimer > 0.0f) return false;
            
            Vector2 direction = Vector2.right * (PlayerFlags.IsFacingRight ? 1.0f : -1.0f);

            bool hit = Physics2D.BoxCast(
                PlayerData.Collider.bounds.center,
                PlayerData.Collider.bounds.size,
                0.0f,
                direction,
                PlayerParameters.WallCheckDistance,
                LayerManager.GroundLayerMask
            );

            return hit;
        }
        
        public bool OnEdge()
        {
            float directionX = PlayerFlags.IsFacingRight ? 1.0f : -1.0f;
            Vector2 direction = Vector2.right * directionX;

            bool previousQueries = Physics2D.queriesStartInColliders;
            Physics2D.queriesStartInColliders = false;

            float originX = PlayerData.Collider.bounds.center.x;
            float castDistance = PlayerParameters.WallCheckDistance + PlayerData.Collider.bounds.extents.x;
            
            Vector2 bottomOrigin = new Vector2(
                originX, 
                PlayerData.Collider.bounds.center.y + (PlayerData.Collider.bounds.extents.y * 0.8f)
            );

            Vector2 topOrigin = new Vector2(
                originX, 
                PlayerData.Collider.bounds.max.y + 0.2f
            );

            RaycastHit2D bottomHit = Physics2D.Raycast(
                bottomOrigin, 
                direction, 
                castDistance, 
                LayerManager.GroundLayerMask
            );
    
            RaycastHit2D topHit = Physics2D.BoxCast(
                topOrigin, 
                new Vector2(0.1f, 0.2f), 
                0f, 
                direction, 
                castDistance, 
                LayerManager.GroundLayerMask
            );
            
            Physics2D.queriesStartInColliders = previousQueries;

            if (bottomHit.collider && !topHit.collider)
            {
                PlayerEdgeClimbData.EdgeHit = bottomHit;
                return true;
            }

            return false;
        } 
        
        #endregion

        #region Get Methods
        
        public Vector2 GetLedgePos()
        {
            if (!PlayerEdgeClimbData.EdgeHit.collider) return Vector2.zero;
            
            float direction = PlayerFlags.IsFacingRight ? 1.0f : -1.0f;
            float wallX = PlayerEdgeClimbData.EdgeHit.point.x;

            Vector2 rayStart = new Vector2(
                wallX + (direction * 0.2f),
                PlayerEdgeClimbData.EdgeHit.point.y + 1.5f
            );

            RaycastHit2D topSurfaceHit = Physics2D.Raycast(
                rayStart,
                Vector2.down,
                2.0f,
                LayerManager.GroundLayerMask
            );
            
            float surfaceY = topSurfaceHit.collider ? topSurfaceHit.point.y : PlayerEdgeClimbData.EdgeHit.point.y;
            
            float offsetX = 0.5f;
            float targetX = wallX + (direction * (PlayerData.Collider.bounds.extents.x + offsetX));

            float offsetY = 0.5f;
            float targetY = surfaceY + PlayerData.Collider.bounds.extents.y + offsetY;
            
            return new Vector2(targetX, targetY);
        }
        
        #endregion
    }
}
