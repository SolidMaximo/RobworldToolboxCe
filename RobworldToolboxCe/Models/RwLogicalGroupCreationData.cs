using Tecnomatix.Engineering;

namespace RobworldToolboxCe.Models
{
    /// <summary>
    /// Represents the creationData for a logical group
    /// </summary>
    public class RwLogicalGroupCreationData
    {
        /// <summary>
        /// Get or set the name of the logical group
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Get or set the type name of the logical group
        /// </summary>
        public string GroupTypeName { get; set; }

        /// <summary>
        /// Get or set the items that belongs to the logical group
        /// </summary>
        public TxObjectList Items { get; set; }

        /// <summary>
        /// Create a new instance of the logical group cration data
        /// </summary>
        public RwLogicalGroupCreationData()
        {
            Items = new TxObjectList();
        }
    }
}
