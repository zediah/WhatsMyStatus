using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WhatsMyStatus_DnD_.Views.Dialogs
{
    public partial class HealthChangePopup : UserControl
    {
        public HealthChangePopup()
        {
            InitializeComponent();
        }

        public void SetDescription(string newDescription)
        {
            description.Text = newDescription;
        }
    }
}
