using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.ApplicationModel.Store;

namespace WhatsMyStatus_DnD_
{
    public partial class DonationsPage : PhoneApplicationPage
    {
        public DonationsPage()
        {
            InitializeComponent();
        }

        private void level1donation_Click(object sender, RoutedEventArgs e)
        {
            // ***********************************************
            // 			 Method Logic
            // ***********************************************
            try
            {
                PurchaseProduct("Donation Level 1");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
      
        }

        private void level2donation_Click(object sender, RoutedEventArgs e)
        {
            // ***********************************************
            // 			 Method Logic
            // ***********************************************
            try
            {
                PurchaseProduct("Donation Level 2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
      
        }

        private void level3donation_Click(object sender, RoutedEventArgs e)
        {
            // ***********************************************
            // 			 Method Logic
            // ***********************************************
            try
            {
                PurchaseProduct("Maximum Donation Level");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
      
        }

        async void PurchaseProduct(string productId)
        {
            try
            {
                // Kick off purchase; don't ask for a receipt when it returns
                await CurrentApp.RequestProductPurchaseAsync(productId, false);

                MessageBox.Show(
                    "Thank you very much for the support! It really means a lot that you find this app worthy of spending your hard earned money on. I am, as always, at your service." +
                    Environment.NewLine + "- Zediah", "Wow. Such amaze. So great. Wow.", MessageBoxButton.OK);

            }
            catch (Exception ex)
            {
                // When the user does not complete the purchase (e.g. cancels or navigates back from the Purchase Page), an exception with an HRESULT of E_FAIL is expected.
            }
        }
    }
}