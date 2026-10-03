using UnityEngine;

namespace PlayerFol.PlayerDataStructs
{
    public class PlayerData
    {
        public Camera Camera { get; private set; }
        
        public Transform PlayerTransform { get; private set; }
        
        public Transform GroundCheckPoint { get; private set; }
        
        public Rigidbody2D Rigidbody { get; private set; }
        
        public Vector2 MoveInput { get; set; }
        public Vector2 ExternalForce { get; set; }
        
        public BoxCollider2D Collider { get; private set; }
        
        public HitBoxCollider HitBox { get; private set; }

        public PlayerData(Transform playerTransform, Transform groundCheckPoint, Rigidbody2D rigidbody,
            BoxCollider2D collider, Camera cam, HitBoxCollider hitBox)
        {
            PlayerTransform = playerTransform;
            GroundCheckPoint = groundCheckPoint;
            Rigidbody = rigidbody;
            Collider = collider;
            Camera = cam;
            HitBox = hitBox;
        }
    }
}
