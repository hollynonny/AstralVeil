using UnityEngine;

namespace PlayerFol
{
    public class PlayerAnimation
    {
        private static readonly int XForceHash = Animator.StringToHash("XForce");
        private static readonly int YForceHash = Animator.StringToHash("YForce");
        private static readonly int IsDashingHash = Animator.StringToHash("IsDashing");
        
        private readonly SpriteRenderer _spriteRenderer;
        private readonly Animator _anim;

        private readonly Material _defaultMaterial;
        private readonly Material _idleMaterial;
        private readonly Material _runMaterial;
        private readonly Material _jumpMaterial;
        private readonly Material _fallMaterial;
        private readonly Material _dashMaterial;
        
        public PlayerAnimation(Animator animator, SpriteRenderer spriteRenderer)
        {
            _anim = animator;
            _spriteRenderer = spriteRenderer;

            _defaultMaterial = Resources.Load<Material>("Materials/Player_Default_Lights");
            _idleMaterial = Resources.Load<Material>("Materials/Player_Idle_Lights");
            _runMaterial = Resources.Load<Material>("Materials/Player_Run_Lights");
            _jumpMaterial = Resources.Load<Material>("Materials/Player_Jump_Lights");
            _dashMaterial = Resources.Load<Material>("Materials/Player_Dash_Lights");
            _fallMaterial = Resources.Load<Material>("Materials/Player_Fall_Lights");
        }

        public void SetMoveAnimation(float xForce)
        {
            _anim.SetFloat(XForceHash, xForce);
        }

        public void SetJumpAnimation(float yForce)
        {
            _anim.SetFloat(YForceHash, yForce);
        }

        public void SetDashAnimation(bool isDashing)
        {
            _anim.SetBool(IsDashingHash, isDashing);
        }

        public void SetDefaultMaterial()
        {
            SetMaterial(_defaultMaterial);
        }

        public void SetIdleMaterial()
        {
            SetMaterial(_idleMaterial);
        }

        public void SetRunMaterial()
        {
            SetMaterial(_runMaterial);
        }

        public void SetJumpMaterial()
        {
            SetMaterial(_jumpMaterial);
        }

        public void SetDashMaterial()
        {
            SetMaterial(_dashMaterial);
        }

        public void SetFallMaterial()
        {
            SetMaterial(_fallMaterial);
        }

        private void SetMaterial(Material material)
        {
            if (material != null)
            {
                _spriteRenderer.material = material;
            }
        }
    }
}
