using UnityEngine;

namespace Managers
{
    public sealed class FpsCounter : MonoBehaviour
    {
        [SerializeField] private float updateInterval = 0.5f;
        [SerializeField] private bool showInRelease = true;

        private float _accum;
        private int _frames;
        private float _timeLeft;
        private float _fps;
        private float _worstMs;
        private GUIStyle _style;

        private void Awake()
        {
            if (!Debug.isDebugBuild && !showInRelease)
            {
                enabled = false;
                return;
            }

            _timeLeft = updateInterval;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            float dt = Time.unscaledDeltaTime;

            _accum += dt;
            _frames++;
            _timeLeft -= dt;
            _worstMs = Mathf.Max(_worstMs, dt * 1000f);

            if (_timeLeft > 0f) return;

            _fps = _frames / _accum;
            _timeLeft = updateInterval;
            _accum = 0f;
            _frames = 0;
        }

        private void LateUpdate()
        {
            // сбрасываем худший кадр после показа
            if (_timeLeft == updateInterval) _worstMs = 0f;
        }

        private void OnGUI()
        {
            if (_style == null)
            {
                _style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 18,
                    fontStyle = FontStyle.Bold
                };
                _style.normal.textColor = Color.white;
            }

            GUI.Label(new Rect(10, 10, 300, 50),
                $"{_fps:F0} FPS  (worst {_worstMs:F1} ms)", _style);
        }
    }
}