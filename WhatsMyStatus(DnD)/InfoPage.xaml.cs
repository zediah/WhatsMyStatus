using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using WhatsMyStatus_DnD_.ViewModels;
using WhatsMyStatus_DnD_.Views.Dialogs;

namespace WhatsMyStatus_DnD_
{
    public partial class InfoPage : PhoneApplicationPage
    {
        private WmsCharacter CurrentCharacter { get; set; }
        // Constructor
        public InfoPage()
        {
            InitializeComponent();
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var characterId = NavigationContext.QueryString.ContainsKey("id") ?  int.Parse(NavigationContext.QueryString["id"]) : 0;
            
            CurrentCharacter = WmsFakeDb.Database.Characters.FirstOrDefault(x => x.Dbseqnum == characterId);
            if (CurrentCharacter != null)
            {
                var normalHpCvs = new CollectionViewSource();
                normalHpCvs.Source = WmsFakeDb.Database.Combats;
                normalHpCvs.Filter += (sender, args) => args.Accepted = ((WmsCombat)args.Item).ChangeReason != HpChangeReasons.TempHp && ((WmsCombat)args.Item).Character.Dbseqnum == CurrentCharacter.Dbseqnum;

                var tempHpCvs = new CollectionViewSource();
                tempHpCvs.Source = WmsFakeDb.Database.Combats;
                tempHpCvs.Filter += (sender, args) => args.Accepted = ((WmsCombat)args.Item).ChangeReason == HpChangeReasons.TempHp && ((WmsCombat)args.Item).Character.Dbseqnum == CurrentCharacter.Dbseqnum;

                statusListSelector.ItemsSource = WmsFakeDb.Database.Statuses;
                if (!PhoneApplicationService.Current.State.ContainsKey("Character"))
                {
                    PhoneApplicationService.Current.State.Add("Character", CurrentCharacter);
                }
                else
                {
                    PhoneApplicationService.Current.State["Character"] = CurrentCharacter;
                }

                normalCombat.DataContext = normalHpCvs;
                tempCombat.DataContext = tempHpCvs;
            }
            UpdateHpTotals();
        }

        private void UpdateHpTotals()
        {
            if (CurrentCharacter != null)
            {
                hpText.Text = "HP (" + CurrentCharacter.GetCurrentHp() + ")";
                tempHpText.Text = "Temp Hp" + (CurrentCharacter.HasTempHp() ? " (" + CurrentCharacter.GetCurrentTempHp() + ")" : "");

            }
        }

        private void btnHeal_Click(object sender, RoutedEventArgs e)
        {
           if (CurrentCharacter != null)
           {
               ShowPopup(HpChangeReasons.Heal);
           }
        }

        private void ShowPopup(HpChangeReasons reason)
        {
            Popup popup = new Popup();

            var control = new HealthChangePopup();
            switch (reason)
            {
                case HpChangeReasons.TempHp:
                    control.SetDescription("Looks like someone likes you ;).\nHow many fake hit points did you get?");
                    break;
                case HpChangeReasons.Damage:
                    control.SetDescription("Tis but a flesh wound.\nBut how big a flesh wound?");
                    break;
                case HpChangeReasons.Heal:
                    control.SetDescription("Been healed have you?\nHow many points of wounds closed!?");
                    break;
            }

            popup.VerticalOffset = 100;
            popup.HorizontalOffset = 70;

            control.okButton.Click += (o, args) =>
            {
                popup.IsOpen = false;
                if (!string.IsNullOrWhiteSpace(control.value.Text))
                {
                    CurrentCharacter.AddCombat(reason, int.Parse(control.value.Text));
                    UpdateHpTotals();
                }
            };

            control.LostFocus += (sender, args) => popup.IsOpen = false;
            popup.Child = control;
            popup.IsOpen = true;
            control.value.Focus();
        }

        private void btnTempHp_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCharacter != null)
            {
                ShowPopup(HpChangeReasons.TempHp);
            }

        }

        private void btnBeenHit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCharacter != null)
            {
                ShowPopup(HpChangeReasons.Damage);
            }
        }

        private void btn_FullReset_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_RoundOver_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_SpendSurge_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}