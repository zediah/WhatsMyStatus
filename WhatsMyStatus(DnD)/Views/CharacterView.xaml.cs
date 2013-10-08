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

namespace WhatsMyStatus_DnD_.Views
{
    public partial class CharacterView : UserControl
    {
        public CharacterView()
        {
            InitializeComponent();
        }

        private void lb_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            
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

        private void lb_Hold(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            Popup popup = new Popup();

            var control = new CharacterCreationPopup();
            control.Character = (WmsCharacter) lb.SelectedItem;
            control.BindControls();
            popup.VerticalOffset = 100;
            popup.HorizontalOffset = 70;

            control.okButton.Click += (o, args) =>
            {
                if (control.AllFilledIn())
                {
                    //WmsCharacter newChar = new WmsCharacter();
                    //newChar.Name = control.tbName.Text;
                    //newChar.Level = int.Parse(control.tbLevel.Text);
                    //newChar.MaxHp = int.Parse(control.tbMaxHp.Text);
                    //newChar.CharacterClass = control.tbClass.Text;

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
