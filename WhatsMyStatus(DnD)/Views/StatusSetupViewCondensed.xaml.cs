using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using WhatsMyStatus_DnD_.ViewModels;

namespace WhatsMyStatus_DnD_.Views
{
    public partial class StatusSetupViewCondensed : UserControl
    {
        public StatusSetupViewCondensed()
        {
            InitializeComponent();
        }

        private void miRemove_Click(object sender, RoutedEventArgs e)
        {
            var item = (MenuItem)sender;

            var status = item.DataContext as WmsStatus;
            if (status != null)
            {
                WmsFakeDb.Database.Remove(status);
            } 
        }
    }
}
