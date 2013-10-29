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
        public StatusView()
        {
            InitializeComponent();
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

            var character = PhoneApplicationService.Current.State["Character"] as WmsCharacter;
            return character;
        }

        private void CreateCharacterStatus()
        {
            var status = DataContext as WmsStatus;
            if (status != null)
            {
                WmsCharacterStatus charStatus = statusRoundView.DataContext as WmsCharacterStatus;
                if (charStatus == null)
                {
                    charStatus = new WmsCharacterStatus();
                    charStatus.RoundsRemaining = 1;
                    charStatus.Character = GetCharacter();
                    charStatus.Status = status;

                    WmsFakeDb.Database.Add(charStatus);
                    statusRoundView.DataContext = charStatus;
                }
                charStatus.ShowRounds = status.RoundsRequired;
            }
        }

        private void RemoveCharacterStatus()
        {
            var charStatus = statusRoundView.DataContext as WmsCharacterStatus;
            if (charStatus != null)
            {
                statusRoundView.DataContext = null;
                WmsFakeDb.Database.Remove(charStatus);
            }
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            var character = GetCharacter();
            // Try see if our character is already affected by this status
            var charStatus = WmsFakeDb.Database.CharacterStatuses.FirstOrDefault(x => x.Character.Equals(character) && x.Status.Equals((WmsStatus) DataContext));
            if (charStatus != null)
            {
                statusRoundView.DataContext = charStatus;
                cb_ActiveStatus.IsChecked = true;
            }
        }
    }
}
