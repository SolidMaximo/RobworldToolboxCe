using RobworldToolboxCe.CommandEnablers;
using RobworldToolboxCe.Views;
using Tecnomatix.Engineering;

namespace RobworldToolboxCe
{
    /// <summary>
    /// This command can create logical collections from compound parts depending on grouping rules
    /// </summary>
    public class CreateLogicalGroupsCommand : TxButtonCommand
    {
        #region Fields
        private readonly RwAlwaysTrueCommandEnabler enabler;
        #endregion

        #region Properties
        /// <summary>
        /// Get the name of the command
        /// </summary>
        public override string Name
        {
            get { return StringTable.CREATELOGICALGROUPS_NAME; }
        }

        /// <summary>
        /// Get the Category under which the command appears
        /// </summary>
        public override string Category
        {
            get { return StringTable.CREATELOGICALGROUPS_CATEGORY; }
        }

        /// <summary>
        /// Get the description of the command
        /// </summary>
        public override string Description
        {
            get { return StringTable.CREATELOGICALGROUPS_DESCRIPTION; }
        }

        /// <summary>
        /// Shows the tooltip of the command
        /// </summary>
        public override string Tooltip
        {
            get { return StringTable.CREATELOGICALGROUPS_TOOLTIP; }
        }

        /// <summary>
        /// Get the 16x16 bitmap of the command
        /// </summary>
        public override string Bitmap
        {
            get { return StringTable.CREATELOGICALGROUPS_BITMAP; }
        }

        /// <summary>
        /// Get the 32x32 bitmap of the command
        /// </summary>
        public override string LargeBitmap
        {
            get { return StringTable.CREATELOGICALGROUPS_LARGEBITMAP; }
        }

        /// <summary>
        /// Get the command enabler
        /// </summary>
        public override ITxCommandEnabler CommandEnabler
        {
            get { return enabler; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an instance of the command
        /// </summary>
        public CreateLogicalGroupsCommand()
        {
            enabler = new RwAlwaysTrueCommandEnabler();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Execute the selected command
        /// </summary>
        /// <param name="cmdParams"></param>
        public override void Execute(object cmdParams)
        {
            try
            {
                RwCreateLocigalGroupsView view = new RwCreateLocigalGroupsView();
                view.Show();
            }
            catch (TxException ex)
            {
                string caption = "An Exception occured!!";
                TxMessageBox.ShowModal(ex.Message, caption, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Activate the command if a valid license is available
        /// </summary>
        /// <returns>The boolean value of the license check</returns>
        public override bool Connect()
        {
            return true;
        }
        #endregion
    }
}
