using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            Camera cam = Camera.main;
            if (cam != null) cam.orthographicSize = 5.0f;
        }
    }
}
