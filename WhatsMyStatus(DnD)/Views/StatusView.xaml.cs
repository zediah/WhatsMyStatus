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
    public partial class StatusView : UserControl
    {
        public WmsCharacter Character { get; set; }
        public StatusView()
        {
            InitializeComponent();
        }

        private void addRound_Click(object sender, RoutedEventArgs e)
        {
            var charStatus = txt_Remaining.DataContext as WmsCharacterStatus;
            if (charStatus != null)
            {
                charStatus.RoundsRemaining++;
            }
        }

        private void minusRound_Click(object sender, RoutedEventArgs e)
        {
            var charStatus = txt_Remaining.DataContext as WmsCharacterStatus;
            if (charStatus != null)
            {
                charStatus.RoundsRemaining--;
            }
        }

        private void cb_ActiveStatus_Checked(object sender, RoutedEventArgs e)
        {
            if (cb_ActiveStatus.IsChecked.GetValueOrDefault())
            {
                CreateCharacterStatus();
            }
            else
            {
                RemoveCharacterStatus();
            }
        }

        private WmsCharacter GetCharacter()
        {
            return Character;
        }

        private void CreateCharacterStatus()
        {
            WmsCharacterStatus cs = new WmsCharacterStatus();
            cs.RoundsRemaining = 1;
            cs.Character = GetCharacter();
            cs.Status = DataContext as WmsStatus;

            WmsFakeDb.Database.Add(cs);

            txt_Remaining.DataContext = cs;
        }

        private void RemoveCharacterStatus()
        {
            var charStatus = txt_Remaining.DataContext as WmsCharacterStatus;
            if (charStatus != null)
            {
                txt_Remaining.DataContext = null;
                WmsFakeDb.Database.Remove(charStatus);
            }
        }
    }
}
