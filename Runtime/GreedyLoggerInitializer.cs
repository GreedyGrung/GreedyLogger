using UnityEngine;
using GreedyLogger.Settings;

namespace GreedyLogger
{
    public class GreedyLoggerInitializer : MonoBehaviour
    {
        [SerializeField] private LoggingSettings _settings;

        private void Awake()
        {
            if (_settings == null)
            {
                Debug.LogError("No settings found! Logging will not be available!");
            }

            GLogger.Initialize(_settings);
        }
    }
}
