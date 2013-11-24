using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WhatsMyStatus_DnD_.ViewModels;
using WhatsMyStatus_DnD_.Views.Dialogs;

namespace WhatsMyStatus_DnD_
{
    public partial class MainPage : PhoneApplicationPage
    {
        public WmsStatusFilter StatusFilter { get; set; }
        public WmsStatusFilter DefaultsFilter { get; set; }

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

        private void AddStatusClick(object sender, RoutedEventArgs e)
        {
            // If we don't have one selected yet, the user may have entered details and then hit the plus button
            // Thinking that was how it worked...so, fill in the details based on what we see
            if (StatusSetup.DataContext == null)
            {
                
                WmsStatus newStatus = StatusSetup.CreateNewStatusFromSelections();
                WmsFakeDb.Database.Add(newStatus);
                CondensedStatusSelector.SelectedItem = newStatus;
            }
            else
            {
                // Create a new one, it's an observable collection - should be automatically seen in the setup...
                WmsFakeDb.Database.Add(new WmsStatus() { Name = "No Name" });
            }

        }


        private void btn_Donate_Click(object sender, RoutedEventArgs e)
        {
            // ***********************************************
            // 			 Method Logic
            // ***********************************************
            try
            {
                    var mainFrame = Application.Current.RootVisual as PhoneApplicationFrame;
                    mainFrame.Navigate(new Uri("/DonationsPage.xaml?", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
      
        }

        private void btn_StatusFilter_Click(object sender, RoutedEventArgs e)
        {
            // ***********************************************
            // 			 Method Logic
            // ***********************************************
            try
            {
                Popup popup = new Popup();

                var control = new StatusFilterPopup();
                if (StatusFilter == null)
                {
                    StatusFilter = new WmsStatusFilter();
                }
                control.DataContext = StatusFilter;
                popup.VerticalOffset = 100;
                popup.HorizontalOffset = 70;

                control.btn_done.Click += (o, args) =>
                {
                    popup.IsOpen = false;
                    ((ICollectionView) CondensedStatusSelector.ItemsSource).Refresh();
                };

                popup.Child = control;
                popup.IsOpen = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
      
        }

        public void UpdateFilter()
        {
            // ***********************************************
            // 			 Method Logic
            // ***********************************************
            try
            {
                if (Resources.Contains("StatusCv"))
                {
                    var statusCv = Resources["StatusCv"] as CollectionViewSource;
                    statusCv.Filter +=statusCv_Filter;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
      
        }

        public void statusCv_Filter(object sender, FilterEventArgs e)
        {
            // ***********************************************
            // 			 Method Logic
            // ***********************************************
            try
            {
                if (StatusFilter == null)
                {
                    StatusFilter = new WmsStatusFilter();
                }
                var item = e.Item as WmsStatus;
                if (item != null)
                {
                    e.Accepted = !StatusFilter.FilterGameSystem.HasValue || StatusFilter.FilterGameSystem.Value == item.GameSystem;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
      
        }

        private void btn_AddDefaults_Click(object sender, RoutedEventArgs e)
        {
            // ***********************************************
            // 			 Method Logic
            // ***********************************************
            try
            {
                Popup popup = new Popup();

                var control = new StatusFilterPopup();
                if (DefaultsFilter == null)
                {
                    DefaultsFilter = new WmsStatusFilter();
                }
                control.DataContext = DefaultsFilter;
                popup.VerticalOffset = 100;
                popup.HorizontalOffset = 70;

                control.btn_done.Click += (o, args) =>
                {
                    popup.IsOpen = false;
                    WmsStatus.CreateDefaultStatuses(DefaultsFilter.FilterGameSystem.GetValueOrDefault());
                };

                popup.Child = control;
                popup.IsOpen = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
      
        }
    }
}