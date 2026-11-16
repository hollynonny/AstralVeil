using Managers;
using UnityEngine;

namespace EnemyFol.Chaser
{
    public class ChaserEnemy : Enemy
    {
        private const float FaceDeadZone = 0.05f;

        private Vector3 _startPos;
        
        public Rigidbody2D Rigidbody { get; private set; }
        
        public ChaserStates States { get; private set; } 
        
        public Transform Player { get; private set; }

        [SerializeField] private bool spriteFacesRight;

        private float _speed = 3.0f;

        private float _agroDistance = 4.0f;
        public float AgroDistance => _agroDistance * _agroDistance;
        
        public bool HasPlayer => Player != null;
        public int FacingDirection { get; private set; }
        
        public float DistanceFromPlayerSqr => HasPlayer ? (Player.position - transform.position).sqrMagnitude : float.PositiveInfinity;

        public int DirectionToPlayer
        {
            get
            {
                if (!HasPlayer) return 0;
                
                float dx = Player.position.x - transform.position.x;
                if (Mathf.Abs(dx) < FaceDeadZone) return 0;
                return dx > 0f ? 1 : -1;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Rigidbody = GetComponent<Rigidbody2D>();
            
            _startPos = transform.position;
            FacingDirection = (transform.localScale.x > 0f) == spriteFacesRight ? 1 : -1;
        }

        protected override void Start()
        {
            base.Start();
            Player = RoomManager.Instance.Player;
        }
        
        private void FixedUpdate()
        {
            if (IsDead) return;
            StateManager.CurrentState.PhysicsUpdate();
        }

        private void Update()
        {
            if (IsDead) return;
            StateManager.CurrentState.LogicUpdate();
        }

        protected override EnemyState CreateStates()
        {
            States = new ChaserStates(this);
            return States.Idle;
        }

        protected override void Revive()
        {
            base.Revive();

            transform.position = _startPos;
            
            StateManager.ChangeState(States.Idle);
        }

        public void MoveTowardsPlayer()
        {
            transform.position = Vector3.MoveTowards(
                transform.position, 
                Player.position, 
                _speed * Time.deltaTime
            );
        }
        
        public void FacePlayer() => FaceDirection(DirectionToPlayer);

        public void FaceDirection(int direction)
        {
            if (direction == 0 || direction == FacingDirection) return;
            
            FacingDirection = direction;
            
            Vector3 scale = transform.localScale;
            float sign = spriteFacesRight ? direction : -direction;
            scale.x = Mathf.Abs(scale.x) * sign;
            transform.localScale = scale;
        }
    }
}
