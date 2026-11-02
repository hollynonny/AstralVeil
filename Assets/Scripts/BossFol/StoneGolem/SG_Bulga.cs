using Managers;
using UnityEngine;

namespace BossFol.StoneGolem
{
    public class SG_Bulga : MonoBehaviour
    {
        private Transform _parent;
        private Vector3 _localStartPos;
        private Vector3 _localStartScale;
        
        [SerializeField] private float lifeTime = 3.0f;
        
        private Rigidbody2D _rb;

        private float _currentLife;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _parent = transform.parent;
            _localStartPos = transform.localPosition;
            _localStartScale = transform.localScale;
            
            Deactivate();
        }

        private void Update()
        {
            _currentLife -= Time.deltaTime;
            if (_currentLife <= 0)
            {
                Deactivate();
            }
        }

        private void Deactivate()
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0.0f;
            transform.SetParent(_parent);
            gameObject.transform.localPosition = _localStartPos;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = _localStartScale;
                
            gameObject.SetActive(false);
        }
        
        public void ResetState() => Deactivate();

        public void Launch(Vector2 target, float flightTime)
        {
            _currentLife = lifeTime;
            
            transform.SetParent(null);
            transform.localScale = _localStartScale;
            
            Vector2 delta = target - (Vector2)transform.position;
            float g = Physics2D.gravity.y * _rb.gravityScale;
            
            float vx = delta.x / flightTime;
            float vy = (delta.y - 0.5f * g * flightTime * flightTime) / flightTime;
            
            _rb.linearVelocity = new Vector2(vx, vy);
            _rb.angularVelocity = Random.Range(-180.0f, 180.0f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(TagManager.PlayerTag))
            {
                _rb.linearVelocity = Vector2.zero;
                _rb.angularVelocity = 0.0f;
                _rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }
}
