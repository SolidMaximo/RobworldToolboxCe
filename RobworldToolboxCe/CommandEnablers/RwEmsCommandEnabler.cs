using Tecnomatix.Engineering;

namespace RobworldToolboxCe.CommandEnablers
{
    /// <summary>
    /// Enables a command on eMServer platform only
    /// </summary>
    internal class RwEmsCommandEnabler : TxCommandEnabler
    {
        /// <summary>
        /// Create a new instance of an eMServer only command enabler
        /// </summary>
        public RwEmsCommandEnabler()
        {
            _enable = TxApplication.PlatformType == TxPlatformType.EmServer;
        }
    }
}
