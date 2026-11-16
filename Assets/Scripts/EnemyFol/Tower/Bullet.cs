using Managers;
using UnityEngine;

namespace EnemyFol.Tower
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 3.0f;
        private float _timer;

        private void OnEnable()
        {
            _timer = 0.0f;
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= lifeTime)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            
        }
    }
}
