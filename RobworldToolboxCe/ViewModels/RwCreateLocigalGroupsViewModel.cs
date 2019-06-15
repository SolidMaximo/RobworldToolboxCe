using EngineeringInternalExtension.ModelObjects;
using RobworldToolboxCe.Common;
using RobworldToolboxCe.Enumerations;
using RobworldToolboxCe.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.PrivateImplementationDetails;
using Tecnomatix.Planning;

namespace RobworldToolboxCe.ViewModels
{
    /// <summary>
    /// Create logical Groups from a selected compound part
    /// </summary>
    public class RwCreateLocigalGroupsViewModel : RwPropertyChangedNotifier
    {
        #region Fields
        private string logicalGroupsGroupBoxHeader;
        private Dictionary<string, TxObjectList> logicalGroups;
        private bool canEnableSortingRulesInput;
        private string sortingRules;
        private string[] rules;
        #endregion

        #region Properties
        /// <summary>
        /// Get the title of the window
        /// </summary>
        public string Title { get { return "Create logical groups for parts"; } }

        /// <summary>
        /// Get the height of the window
        /// </summary>
        public int WindowHeight { get { return 250; } }

        /// <summary>
        /// Get the width of the window
        /// </summary>
        public int WindowWidth { get { return 250; } }

        /// <summary>
        /// Get the caption of the logical groups data groupbox
        /// </summary>
        public string LogicalGroupsGroupBoxHeader
        {
            get { return logicalGroupsGroupBoxHeader; }
            private set
            {
                if (logicalGroupsGroupBoxHeader != value)
                {
                    logicalGroupsGroupBoxHeader = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get or set the logical groups
        /// </summary>
        public ObservableCollection<RwLogicalGroupCreationData> LogicalGroupsData { get; set; }

        /// <summary>
        /// Get or set the selected sorting mode
        /// </summary>
        public RwLogicalGroupCreationModes SelectedSorting { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool CanEnableSortingRulesInput
        {
            get { return canEnableSortingRulesInput; }
            set
            {
                if (canEnableSortingRulesInput != value)
                {
                    canEnableSortingRulesInput = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get or set the rules for sorting by the part prototype name
        /// </summary>
        public string SortingRules
        {
            get { return sortingRules; }
            set
            {
                if (sortingRules != value)
                {
                    sortingRules = value;
                    CreateSortingRules();
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get the tooltip for the sorting rules input
        /// </summary>
        public string SortingRulesTooltip { get; private set; }

        /// <summary>
        /// Represents the command for selecting the sorting mode
        /// </summary>
        public ICommand SortingRuleCommand { get; private set; }

        /// <summary>
        /// Represents the command for executing the logical groups creation process
        /// </summary>
        public ICommand ExecuteLogicalGroupsCreationCommand { get; private set; }

        /// <summary>
        /// Represents the command for choosing the compund part
        /// </summary>
        public ICommand CompoundPartPickedEvent { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create an instance of the create logical groups command
        /// </summary>
        public RwCreateLocigalGroupsViewModel()
        {
            LogicalGroupsGroupBoxHeader = "Logical Groups";
            SortingRulesTooltip = "Separator is |";
            SelectedSorting = RwLogicalGroupCreationModes.VARIANTNAME;
            CanEnableSortingRulesInput = false;
            LogicalGroupsData = new ObservableCollection<RwLogicalGroupCreationData>();
            SortingRuleCommand = new RwActionCommand(SetSortingRuleExecuted, SetSortingRuleCanExecute);   
            ExecuteLogicalGroupsCreationCommand = new RwActionCommand(LogicalGroupsCreationExecuted, LogicalGroupsCreationCanExecute);
            CompoundPartPickedEvent = new RwActionCommand(CompoundPartPickedExecuted, CompoundPartPickedCanExecute);
            LogicalGroupsData.CollectionChanged += LogicalGroupsDataCollectionChanged;
        }
        #endregion

        #region Command enablers
        /// <summary>
        /// Decide if the command for picking a compound part can be enabled
        /// </summary>
        /// <param name="arg">Not in use</param>
        /// <returns>Returns always true</returns>
        private bool CompoundPartPickedCanExecute(object arg)
        {
            return true;
        }

        /// <summary>
        /// Decide if the radio buttons for the sorting rules can be enabled
        /// </summary>
        /// <param name="arg">Not in use</param>
        /// <returns>Returns always true</returns>
        private bool SetSortingRuleCanExecute(object arg)
        {
            return true;
        }
        
        /// <summary>
        /// Decide if the command for executing the logical groups creation can be enabled
        /// </summary>
        /// <param name="arg">Not in use</param>
        /// <returns>Returns true if a compund part is selected</returns>
        private bool LogicalGroupsCreationCanExecute(object arg)
        {
            return LogicalGroupsData != null && LogicalGroupsData.Count > 0;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set the compound part for the logical groups creation process
        /// </summary>
        /// <param name="obj">The TxObjEditBoxControl of the view</param>
        private void CompoundPartPickedExecuted(object obj)
        {
            if (obj is Tecnomatix.Engineering.Ui.WPF.TxObjEditBoxControl wpfControl)
            {
                TxCompoundPart compoundPart = wpfControl.Object as TxCompoundPart;
                if(compoundPart != null)
                {
                    logicalGroups = new Dictionary<string, TxObjectList>();
                    TraverseCompoundPart(compoundPart);
                    ConvertDictionaryToObservableCollection();
                }
                else
                {
                    LogicalGroupsData.Clear();
                }
            }
        }

        /// <summary>
        /// Convert the dictionary of parts to an Observalble collection
        /// </summary>
        private void ConvertDictionaryToObservableCollection()
        {
            LogicalGroupsData.Clear();
            foreach (KeyValuePair<string, TxObjectList> pair in logicalGroups)
            {
                LogicalGroupsData.Add(new RwLogicalGroupCreationData { GroupName = pair.Key, GroupTypeName = string.Empty, Items = pair.Value });
            }
        }

        /// <summary>
        /// Traverse the Compound part and add the components to the dictionary.
        /// Empty omponents are skipped!
        /// </summary>
        /// <param name="compoundPart">The selected compund part</param>
        private void TraverseCompoundPart(TxCompoundPart compoundPart)
        {
            if (compoundPart.Count > 0)
            {
                foreach (var item in compoundPart)
                {
                    if (item is TxCompoundPart subCompoundPart)
                    {
                        //Recursive call of TraverseCompoundPart
                        TraverseCompoundPart(subCompoundPart);
                    }
                    else if (item is TxComponent component)
                    {
                        SortComponent(component);
                    }
                }
            }
        }

        /// <summary>
        /// Sorts the component to a logical group
        /// </summary>
        /// <param name="component">The current component</param>
        private void SortComponent(TxComponent component)
        {
            if (SelectedSorting == RwLogicalGroupCreationModes.VARIANTNAME)
            {
                SortComponentByVariantName(component);
            }
            else if (SelectedSorting == RwLogicalGroupCreationModes.PROTOTYPENAME)
            {
                SortComponentByPartName(component);
            }
        }

        /// <summary>
        /// Sort the components by the filename of the prototype
        /// </summary>
        /// <param name="component">The current component</param>
        private void SortComponentByPartName(TxComponent component)
        {
            TxStorage storage = component.StorageObject as TxStorage;
            if (storage is TxLibraryStorage && component.Visibility != TxDisplayableObjectVisibility.CannotBeDisplayed && rules != null && rules.Length > 0)
            {
                string filename = Utilities.RwFileAndDirectoryUtilities.GetFilenameWithoutExtension((storage as TxLibraryStorage).FullPath).ToUpper();
                foreach (string rule in rules)
                {
                    if(filename.Contains(rule))
                    {
                        AddToDictionary(rule, component);
                        return;
                    }
                }
                AddToDictionary("EMPTY", component);
            }
        }

        /// <summary>
        /// Sort the components by the variant name column of the resource tree
        /// </summary>
        /// <param name="component">The current component</param>
        private void SortComponentByVariantName(TxComponent component)
        {
            /*
             * Thanks to Siemens API Team for support
             * https://community.plm.automation.siemens.com/t5/Tecnomatix-Developer-Forum/Variant-name/m-p/550867#M1499
            */
            ITxPlanningObject planningObject = component.PlanningRepresentation;
            if (planningObject is ITxPlanningVariantSetAssignable variantSetAssignable && component.Visibility != TxDisplayableObjectVisibility.CannotBeDisplayed)
            {
                ITxPlanningVariantSet variantSet = variantSetAssignable.GetVariantSet();
                if (variantSet != null)
                {
                    AddToDictionary(variantSet.Name, component);
                }
                else
                {
                    AddToDictionary("EMPTY", component);
                }
            }
        }

        /// <summary>
        /// Create the rules for sorting by part name
        /// </summary>
        private void CreateSortingRules()
        {
            rules = null;
            rules = SortingRules.Trim().ToUpper().Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Add a part to the dictionary
        /// </summary>
        /// <param name="key">The variant name of the part</param>
        /// <param name="component">The part as a component</param>
        private void AddToDictionary(string key, TxComponent component)
        {
            if (!logicalGroups.ContainsKey(key))
            {
                logicalGroups.Add(key, new TxObjectList { component });
            }
            else
            {
                logicalGroups[key].Add(component);
            }
        }

        /// <summary>
        /// Set the sorting type
        /// </summary>
        /// <param name="obj">The sorting parameter</param>
        private void SetSortingRuleExecuted(object obj)
        {
            string mode = obj.ToString();
            if (mode.Equals("V"))
            {
                CanEnableSortingRulesInput = false;
                SelectedSorting = RwLogicalGroupCreationModes.VARIANTNAME;
            }
            else
            {
                CanEnableSortingRulesInput = true;
                SelectedSorting = RwLogicalGroupCreationModes.PROTOTYPENAME;
            }
        }

        /// <summary>
        /// Execute the logical groups creation process
        /// </summary>
        /// <param name="obj">Not in use</param>
        private void LogicalGroupsCreationExecuted(object obj)
        {
            foreach (RwLogicalGroupCreationData data in LogicalGroupsData)
            {
                TxLogicalGroup group = TxApplication.ActiveDocument.LogicalRoot.CreateLogicalGroup(new TxLogicalGroupCreationData { Name = data.GroupName, TypeName = data.GroupTypeName });
                group.AddObjects(data.Items);
            }
        }

        /// <summary>
        /// Update the groupbox header of the logical groups collection
        /// </summary>
        /// <param name="sender">The logical groups collection</param>
        /// <param name="e">The collection changed event arguments</param>
        private void LogicalGroupsDataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (LogicalGroupsData != null)
            {
                LogicalGroupsGroupBoxHeader = $"({LogicalGroupsData.Count}) logical group(s) to create";
            }
        }
        #endregion
    }
}
