using System;
using UnityEngine;

namespace PlayerFol
{
    public class HitBoxCollider : MonoBehaviour
    {
        public event Action<Collider2D> OnTargetHit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTargetHit?.Invoke(other);
        }
    }
}
