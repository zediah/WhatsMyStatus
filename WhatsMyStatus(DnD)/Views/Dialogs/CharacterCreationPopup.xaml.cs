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

namespace WhatsMyStatus_DnD_.Views.Dialogs
{
    public partial class CharacterCreationPopup : UserControl
    {
        public WmsCharacter Character { get; set; }

        public IEnumerable<E_GameSystems> GameSystemses
        {
            get { return Enum.GetValues(typeof (E_GameSystems)).Cast<E_GameSystems>(); }
        }

        public CharacterCreationPopup()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Whether or not all the fields were filled in.
        /// </summary>
        /// <returns></returns>
        public bool AllFilledIn()
        {
            return !string.IsNullOrWhiteSpace(tbClass.Text) &&
                   !string.IsNullOrWhiteSpace(tbLevel.Text) &&
                   !string.IsNullOrWhiteSpace(tbMaxHp.Text) &&
                   !string.IsNullOrWhiteSpace(tbName.Text);
        }

        public void BindControls()
        {
            if (Character != null)
            {
                tbClass.DataContext = Character;
                tbLevel.DataContext = Character;
                tbMaxHp.DataContext = Character;
                tbName.DataContext = Character;
                ccGameSystem.ItemsSource = GameSystemses;
            }
        }


    }
}
