using UnityEngine;

namespace Utilities
{
    public class LoggerGroup : ILogger
    {
        
        [SerializeField] private ILogger[] _loggers;
        [SerializeField] private bool _active;
        
        public override void Active(bool active)
        {
            foreach (ILogger logger in _loggers)
            {
                logger.Active(active);
            }
        }

        private void Update() => Active(_active);

    }
}
