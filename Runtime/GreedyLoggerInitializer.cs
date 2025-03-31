using UnityEngine;
using GreedyLogger.Settings;

namespace GreedyLogger
{
    [DisallowMultipleComponent]
    public class GreedyLoggerInitializer : MonoBehaviour
    {
        private static GreedyLoggerInitializer _instance;

        [SerializeField] private LoggingSettings _settings;

        private bool _isInitialized;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);

                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);

            if (_settings == null)
            {
                Debug.LogError("No settings found! Logging will not be available!");

                return;
            }

            GLogger.Initialize(_settings);
            FileLogger.Initialize(_settings);

            _isInitialized = true;
        }

        private void OnDestroy()
        {
            if (_isInitialized)
            {
                FileLogger.Shutdown();
            }
        }
    }
}
