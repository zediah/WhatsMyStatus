using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WhatsMyStatus_DnD_.ViewModels;

namespace WhatsMyStatus_DnD_.Views
{
    public partial class StatusSetupView : UserControl
    {
        

        public ObservableCollection<E_GameSystems> GameSystemses
        {
            get { return new ObservableCollection<E_GameSystems>(Enum.GetValues(typeof(E_GameSystems)).Cast<E_GameSystems>()); }
        }

        public ObservableCollection<E_StatusEndingCondition> StatusEndingConditions
        {
            get { return new ObservableCollection<E_StatusEndingCondition>(Enum.GetValues(typeof(E_StatusEndingCondition)).Cast<E_StatusEndingCondition>()); }
        }

        public StatusSetupView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Based off the current values in the setup, creates and returns a status (but doesn't add it yet)
        /// </summary>
        /// <returns></returns>
        public WmsStatus CreateNewStatusFromSelections()
        {
            WmsStatus returnValue = null;
            // ***********************************************
            // 			 Method Logic
            // ***********************************************
            try
            {
                returnValue = new WmsStatus();
                returnValue.Name = string.IsNullOrWhiteSpace(statusName.Text) ? "No Name" : statusName.Text;
                returnValue.GameSystem = GameSys.SelectedItem == null ? default(E_GameSystems) : (E_GameSystems)GameSys.SelectedItem;
                returnValue.StatusEndingCondition = EndingCondition.SelectedItem == null ? default(E_StatusEndingCondition) : (E_StatusEndingCondition) EndingCondition.SelectedItem;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return returnValue;
        }
    }
}
