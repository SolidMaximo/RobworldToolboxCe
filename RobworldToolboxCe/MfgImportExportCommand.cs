using RobworldToolboxCe.CommandEnablers;
using RobworldToolboxCe.Views;
using Tecnomatix.Engineering;

namespace RobworldToolboxCe
{
    /// <summary>
    /// This command can export Mfg data to a file or import Mfg data from a file
    /// </summary>
    public class MfgImportExportCommand : TxCompoundButtonCommand
    {
        #region Fields
        private ITxCompoundButtonElement defaultElement;
        private readonly RwEmsCommandEnabler enabler;
        #endregion

        #region Properties
        /// <summary>
        /// Get the Category under which the command appears
        /// </summary>
        public override string Category
        {
            get { return StringTable.MFGIMPORTEXPORT_CATEGORY; }
        }

        /// <summary>
        /// Get the default button of the compound button command
        /// </summary>
        public override ITxCompoundButtonElement DefaultElement
        {
            get { return defaultElement; }
        }

        /// <summary>
        /// Get the name of the command
        /// </summary>
        public override string Name
        {
            get { return StringTable.MFGIMPORTEXPORT_NAME; }
        }

        /// <summary>
        /// Get the 16x16 bitmap of the command
        /// </summary>
        public override string Bitmap
        {
            get { return StringTable.MFGIMPORTEXPORT_EXPORT_BITMAP; }
        }

        /// <summary>
        /// Get the 32x32 bitmap of the command
        /// </summary>
        public override string LargeBitmap
        {
            get { return StringTable.MFGIMPORTEXPORT_EXPORT_LARGEBITMAP; }
        }

        /// <summary>
        /// Get the collection of button commands
        /// </summary>
        public override ITxCompoundButtonRowCollection Rows
        {
            get
            {
                ITxCompoundButtonElement element1 = new TxCompoundButtonElement(StringTable.MFGIMPORTEXPORT_EXPORT_NAME, StringTable.MFGIMPORTEXPORT_EXPORT_DESCRIPTION, StringTable.MFGIMPORTEXPORT_EXPORT_TOOLTIP, StringTable.MFGIMPORTEXPORT_EXPORT_BITMAP, StringTable.MFGIMPORTEXPORT_EXPORT_LARGEBITMAP);
                ITxCompoundButtonElement element2 = new TxCompoundButtonElement(StringTable.MFGIMPORTEXPORT_IMPORT_NAME, StringTable.MFGIMPORTEXPORT_IMPORT_DESCRIPTION, StringTable.MFGIMPORTEXPORT_IMPORT_TOOLTIP, StringTable.MFGIMPORTEXPORT_IMPORT_BITMAP, StringTable.MFGIMPORTEXPORT_IMPORT_LARGEBITMAP);
                TxCompoundButtonElementCollection elements = new TxCompoundButtonElementCollection();
                elements.AddItem(element1);
                elements.AddItem(element2);
                ITxCompoundButtonRow row = new TxCompoundButtonRow(elements);
                TxCompoundButtonRowCollection rows = new TxCompoundButtonRowCollection();
                rows.AddItem(row);
                defaultElement = element1;
                return rows;
            }
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
        public MfgImportExportCommand()
        {
            enabler = new RwEmsCommandEnabler();
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
                if (cmdParams is ITxCompoundButtonElement element)
                {
                    switch (element.Name.ToUpper())
                    {
                        //Cases are similar to the name strings in StringTable.resx
                        case "MFGEXPORT":
                            RwMfgExportView exportView = new RwMfgExportView();
                            exportView.Show();
                            break;
                        case "MFGIMPORT":
                            RwMfgImportView importView = new RwMfgImportView();
                            importView.Show();
                            break;
                        default:
                            break;
                    }
                }
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
