using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public WmsCharacter CurrentCharacter { get; set; }
        // Constructor
        public InfoPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Lets load the data when we're entering this section for each character
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var characterId = NavigationContext.QueryString.ContainsKey("id") ?  int.Parse(NavigationContext.QueryString["id"]) : 0;
            
            CurrentCharacter = WmsFakeDb.Database.Characters.FirstOrDefault(x => x.Dbseqnum == characterId);
            if (CurrentCharacter != null)
            {
                var normalHpCvs = new CollectionViewSource();
                normalHpCvs.Source = CurrentCharacter.Combats;
                normalHpCvs.Filter += (sender, args) => args.Accepted = ((WmsCombat)args.Item).ChangeReason != HpChangeReasons.TempHp && ((WmsCombat)args.Item).Character == CurrentCharacter;

                var tempHpCvs = new CollectionViewSource();
                tempHpCvs.Source = CurrentCharacter.Combats;
                tempHpCvs.Filter += (sender, args) => args.Accepted = ((WmsCombat)args.Item).ChangeReason == HpChangeReasons.TempHp && ((WmsCombat)args.Item).Character == CurrentCharacter;

                CurrentCharacter.CreateMissingCharacterStatuses();

                var statusCvs = new CollectionViewSource();
                statusCvs.Source = CurrentCharacter.CharacterStatuses;
                statusCvs.SortDescriptions.Add(new SortDescription("AfflictedWithStatus", ListSortDirection.Descending));

                
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

                Binding b = new Binding();
                b.Source = statusCvs;

                listB.SetBinding(ListBox.ItemsSourceProperty,b);
            }
            UpdateHpTotals();
        }

        /// <summary>
        /// Visually update the hp total headers
        /// </summary>
        private void UpdateHpTotals()
        {
            if (CurrentCharacter != null)
            {
                hpText.Text = "HP (" + CurrentCharacter.GetCurrentHp() + ")";
                tempHpText.Text = "Temp Hp" + (CurrentCharacter.HasTempHp() ? " (" + CurrentCharacter.GetCurrentTempHp() + ")" : "");

            }
        }

        /// <summary>
        /// Heal is clicked, show the popup and heal!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHeal_Click(object sender, RoutedEventArgs e)
        {
           if (CurrentCharacter != null)
           {
               ShowCombatPopup(HpChangeReasons.Heal);
           }
        }

        /// <summary>
        /// Show a popup based on the reason given, combat will be created from this
        /// </summary>
        /// <param name="reason"></param>
        private void ShowCombatPopup(HpChangeReasons reason)
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
                    control.SetDescription("Yours arms off! No it isn't.\nHow many arms did you heal?");
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
                ShowCombatPopup(HpChangeReasons.TempHp);
            }

        }

        private void btnBeenHit_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCharacter != null)
            {
                ShowCombatPopup(HpChangeReasons.Damage);
            }
        }

        private void btn_FullReset_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCharacter != null)
            {
                if (MessageBox.Show("Are you sure you wish to reset everything?", "Are you sure?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    foreach (var statuse in CurrentCharacter.GetAfflictedStatuses())
                    {
                        WmsFakeDb.Database.Remove(statuse);
                    }
                    foreach (var combat in CurrentCharacter.GetCombats())
                    {
                        WmsFakeDb.Database.Remove(combat);
                    }
                    CurrentCharacter.Surges = 0;
                }
            }
        }

        private void btn_RoundOver_Click(object sender, RoutedEventArgs e)
        {
            var statusesWithRounds = CurrentCharacter.GetAfflictedStatuses().Where(x => x.Status.StatusEndingCondition == E_StatusEndingCondition.Rounds);
            foreach(var charStatus in statusesWithRounds)
            {
                charStatus.RoundsRemaining--;
            }
        }

        private void btn_spendSurge_Click_1(object sender, RoutedEventArgs e)
        {
            if (CurrentCharacter != null)
            {
                CurrentCharacter.SpendSurge();
            }
        }

        private void btn_spendSurgePlusBonus_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCharacter != null)
            {
                Popup popup = new Popup();

                var control = new HealthChangePopup();
                control.SetDescription("");

                popup.VerticalOffset = 100;
                popup.HorizontalOffset = 70;

                control.okButton.Click += (o, args) =>
                {
                    popup.IsOpen = false;
                    if (!string.IsNullOrWhiteSpace(control.value.Text))
                    {
                        CurrentCharacter.SpendSurge(int.Parse(control.value.Text));
                    }
                };

                control.LostFocus += (s, args) => popup.IsOpen = false;
                popup.Child = control;
                popup.IsOpen = true;
                control.value.Focus();

            }
        }

        private void btn_RemoveSurge_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCharacter != null)
            {
                CurrentCharacter.RemoveSurge();
            }
        }

        private void btn_AddSurge_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentCharacter != null)
            {
                CurrentCharacter.AddSurge();
            }
        }
    }
}