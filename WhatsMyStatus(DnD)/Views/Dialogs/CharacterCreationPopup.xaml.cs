using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WhatsMyStatus_DnD_.Views.Dialogs
{
    public partial class CharacterCreationPopup : UserControl
    {
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
    }
}
