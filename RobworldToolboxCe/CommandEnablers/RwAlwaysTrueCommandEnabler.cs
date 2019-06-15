using Tecnomatix.Engineering;

namespace RobworldToolboxCe.CommandEnablers
{
    /// <summary>
    /// Command enabler class that is always true
    /// </summary>
    internal class RwAlwaysTrueCommandEnabler : TxCommandEnabler
    {
        #region Constructors
        /// <summary>
        /// Create an enabler that is always true
        /// </summary>
        internal RwAlwaysTrueCommandEnabler()
        {
            _enable = true;
        }
        #endregion
    }
}
