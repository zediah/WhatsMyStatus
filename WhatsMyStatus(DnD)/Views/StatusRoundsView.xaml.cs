using System;
using System.Collections.Generic;
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
    public partial class StatusRoundsView : UserControl
    {
        public StatusRoundsView()
        {
            InitializeComponent();
        }

        private void addRound_Click(object sender, RoutedEventArgs e)
        {
            var charStatus = DataContext as WmsCharacterStatus;
            if (charStatus != null)
            {
                charStatus.RoundsRemaining++;
            }
        }

        private void minusRound_Click(object sender, RoutedEventArgs e)
        {
            var charStatus = DataContext as WmsCharacterStatus;
            if (charStatus != null)
            {
                charStatus.RoundsRemaining--;
            }
        }
    }
}
