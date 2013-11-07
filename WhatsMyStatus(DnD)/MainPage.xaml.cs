using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WhatsMyStatus_DnD_.ViewModels;
using WhatsMyStatus_DnD_.Views.Dialogs;

namespace WhatsMyStatus_DnD_
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (!WmsFakeDb.Database.IsDataLoaded)
            {
                WmsFakeDb.Database.SetupFromIsolatedStorage();
                WmsFakeDb.Database.IsDataLoaded = true;
            }

            CharacterViewOnPage.DataContext = WmsFakeDb.Database.Characters;
        }

        private void AddCharacterClick(object sender, RoutedEventArgs e)
        {
            Popup popup = new Popup();

            var control = new CharacterCreationPopup();
            var newChar = new WmsCharacter();
            control.Character = newChar;
            control.BindControls();
            popup.VerticalOffset = 100;
            popup.HorizontalOffset = 70;

            control.okButton.Click += (o, args) =>
            {
                if (control.AllFilledIn())
                {
                    WmsFakeDb.Database.Add(newChar);
                    MessageBox.Show("Character Creation Succesful.\n" + newChar.Name + " is ready to rumble!");
                }
                else
                {
                    MessageBox.Show("Character creation unsuccesful.\nNot all fields filled in.");
                }
                popup.IsOpen = false;
            };

            //control.LostFocus += (s, args) => popup.IsOpen = false;
            popup.Child = control;
            popup.IsOpen = true;
            control.tbName.Focus();
        }
    }
}