using Managers;
using UnityEngine;

namespace BossFol.StoneGolem
{
    // this class is a fucking slave
    public sealed class SG_HurtboxCore : MonoBehaviour, IDamageable
    {
        private BossController _owner;

        private void Awake()
        {
            _owner = GetComponentInParent<BossController>();
            if(_owner == null)
                Debug.LogError("No boss state found on " + gameObject.name);
        }

        public void TakeDamage(int damage)
        {
            if (_owner == null) return;
            _owner.TakeDamage(damage);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == LayerManager.PlayerLayer)
            {
                TakeDamage(1);
            }
        }
    }
}
