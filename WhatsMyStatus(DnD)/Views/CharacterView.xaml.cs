﻿using System;
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

namespace WhatsMyStatus_DnD_.Views
{
    public partial class CharacterView : UserControl
    {
        public CharacterView()
        {
            InitializeComponent();
        }

        private void lb_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var selectedItem = lb.SelectedItem as WmsCharacter;
            if (selectedItem != null)
            {
                var mainFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                mainFrame.Navigate(new Uri("/InfoPage.xaml?id=" + selectedItem.Dbseqnum, UriKind.Relative));
            }
        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            Popup popup = new Popup();

            var control = new CharacterCreationPopup();
            control.Character = ((MenuItem) sender).DataContext as WmsCharacter;
            popup.VerticalOffset = 100;
            popup.HorizontalOffset = 70;

            control.okButton.Click += (o, args) =>
            {
                if (control.AllFilledIn())
                {
                    MessageBox.Show("Edit succesful.");
                }
                else
                {
                    MessageBox.Show("Character creation unsuccesful.\nNot all fields filled in.");
                }
                popup.IsOpen = false;
            };

            popup.Child = control;
            popup.IsOpen = true;
            control.tbName.Focus();
        }

        private void miRemove_Click(object sender, RoutedEventArgs e)
        {
            var item = (MenuItem) sender;

            var character = item.DataContext as WmsCharacter;
            if (character != null)
            {
                WmsFakeDb.Database.Remove(character);
            }
        }



    }
}
