using UnityEngine;
using UnityEngine.Rendering;

namespace EnemyFol.Tower
{
    public enum LookDirection
    {
        Up = 1,
        Down = 2,
        Right = 3,
        Left = 4
    }
    
    public class TowerEnemy : Enemy
    {
        public TowerEnemyStates States { get; private set; }

        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private LookDirection lookDirection = LookDirection.Right;
        [SerializeField] private byte poolSize = 10;
        [SerializeField] private float bulletSpeed = 8.0f;
        [SerializeField] private float shootInterval = 1.0f;
        [SerializeField] private Transform firePoint;
        
        private GameObject[] _prefabs;
        private float _shootTimer;

        protected override void Start()
        {
            base.Start();
            InitPool();
        }

        private void InitPool()
        {
            _prefabs = new GameObject[poolSize];
            for (int i = 0; i < poolSize; i++)
            {
                _prefabs[i] = Instantiate(bulletPrefab);
                _prefabs[i].SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            if (IsDead) return;
            
            _shootTimer += Time.fixedDeltaTime;
            if (_shootTimer >= shootInterval)
            {
                _shootTimer = 0.0f;
                Shoot();
            }
        }

        private void Shoot()
        {
            GameObject bullet = PoolObj();
            if (bullet == null) return;
            
            bullet.transform.position = firePoint != null ? firePoint.position : transform.position;
            bullet.SetActive(true);

            Vector2 direction = GetDirectionVector();
            
            if (bullet.TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.linearVelocity = direction * bulletSpeed;
            }
        }

        private GameObject PoolObj()
        {
            for (int i = 0; i < poolSize; i++)
            {
                if (!_prefabs[i].activeInHierarchy)
                {
                    return _prefabs[i];
                }
            }

            return null;
        }

        private Vector2 GetDirectionVector()
        {
            switch (lookDirection)
            {
                case LookDirection.Up:
                    return Vector2.up;
                case LookDirection.Down:
                    return Vector2.down;
                case LookDirection.Left:
                    return Vector2.left;
                case LookDirection.Right:
                    return Vector2.right;
                default:
                    return Vector2.right;
            }
        }
        
        protected override EnemyState CreateStates()
        {
            States = new TowerEnemyStates(this);
            return States.Idle;
        }
    }
}
