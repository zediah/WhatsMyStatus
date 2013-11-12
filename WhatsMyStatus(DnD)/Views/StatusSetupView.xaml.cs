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

        public StatusSetupView()
        {
            InitializeComponent();
        }
    }
}
