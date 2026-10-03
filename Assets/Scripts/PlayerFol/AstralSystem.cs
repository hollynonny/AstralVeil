using System;
using UnityEngine;

namespace PlayerFol
{
    public class AstralSystem
    {
        public bool IsAstral { get; private set; }
        
        public event Action AstralStateChanged;

        public float GravityMultiplier => IsAstral ? 0.4f : 1.0f;
        public float MovementMultiplier => IsAstral ? 0.5f : 1.0f;
        public float JumpMultiplier => IsAstral ? 0.5f : 1.0f;
        
        public AstralSystem()
        {
            IsAstral = false;
        }

        public void EnterAstralState()
        {
            IsAstral = true;
            AstralStateChanged?.Invoke();
            // I'll write some more code here, so this method is going to be useful in future!
        }

        public void ExitAstralState()
        {
            IsAstral = false;
            AstralStateChanged?.Invoke();
            // I'll write some more code here, so this method is going to be useful in future!
        }
    }
}
