using UnityEngine;

namespace PlayerFol
{
    public class PlayerAnimation : MonoBehaviour
    {
        private static readonly int XForceHash = Animator.StringToHash("XForce");
        private static readonly int YForceHash = Animator.StringToHash("YForce");
        private static readonly int IsDashingHash = Animator.StringToHash("IsDashing");
        private static readonly int IsWalling = Animator.StringToHash("IsWalling");
        
        private SpriteRenderer _spriteRenderer;
        private Animator _anim;

        private Material _defaultMaterial;
        private Material _idleMaterial;
        private Material _runMaterial;
        private Material _jumpMaterial;
        private Material _fallMaterial;
        private Material _dashMaterial;
        private Material _wallingMaterial;

        public void Awake()
        {
            _anim = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _defaultMaterial = Resources.Load<Material>("Materials/Player_Default_Lights");
            _idleMaterial = Resources.Load<Material>("Materials/Player_Idle_Lights");
            _runMaterial = Resources.Load<Material>("Materials/Player_Run_Lights");
            _jumpMaterial = Resources.Load<Material>("Materials/Player_Jump_Lights");
            _dashMaterial = Resources.Load<Material>("Materials/Player_Dash_Lights");
            _fallMaterial = Resources.Load<Material>("Materials/Player_Fall_Lights");
            _wallingMaterial = Resources.Load<Material>("Materials/Player_Walling_Lights");
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

        public void SetWallingAnimation(bool isWalling)
        {
            _anim.SetBool(IsWalling, isWalling);
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

        public void SetWallingMaterial()
        {
            SetMaterial(_wallingMaterial);
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
