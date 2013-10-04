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
    public partial class CharacterView : UserControl
    {
        public CharacterView()
        {
            InitializeComponent();
        }

        private void lb_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = lb.SelectedItem as WmsCharacter;
            if (selectedItem != null)
            {
                var mainFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                mainFrame.Navigate(new Uri("/InfoPage.xaml?id=" + selectedItem.Dbseqnum, UriKind.Relative));
            }
        }



    }
}
