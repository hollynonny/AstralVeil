using UnityEngine;

namespace CameraFol
{
    public class CameraEffects : MonoBehaviour
    {
        public static CameraEffects Instance;
        
        private ParticleSystem _pS;

        private void Awake()
        {
            Instance = this;
            
            _pS = GetComponentInChildren<ParticleSystem>();
            Debug.Log(_pS.gameObject.name);
            _pS.Stop();
        }

        public void StartEmitting()
        {
            _pS.Play();
        }

        public void StopEmitting()
        {
            _pS.Stop();
        }
    }
}
