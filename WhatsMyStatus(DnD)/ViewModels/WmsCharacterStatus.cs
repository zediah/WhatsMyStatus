using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsMyStatus_DnD_.ViewModels.Core;

namespace WhatsMyStatus_DnD_.ViewModels
{
    public class WmsCharacterStatus : WmsPrimaryObject, INotifyPropertyChanged
    {

        private int _roundsRemaining;

        /// <summary>
        /// This is the number of rounds remaining the the status should be in effect.
        /// </summary>
        public int RoundsRemaining
        {
            get { return _roundsRemaining; }
            set
            {
                if (_roundsRemaining != value)
                {
                    _roundsRemaining = value;
                    NotifyPropertyChanged("RoundsRemaining");
                }
            }
        }

        private WmsStatus _status;

        /// <summary>
        /// The status this is for
        /// </summary>
        public WmsStatus Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    NotifyPropertyChanged("State");
                }
            }
        }

        private WmsCharacter _character;

        /// <summary>
        /// The character this is for
        /// </summary>
        public WmsCharacter Character
        {
            get { return _character; }
            set
            {
                if (_character != value)
                {
                    _character = value;
                    NotifyPropertyChanged(("Character"));
                }
            }
        }

        private bool _showRounds;

        public bool ShowRounds
        {
            get { return _showRounds; }
            set
            {
                if (_showRounds != value)
                {
                    _showRounds = value;
                    NotifyPropertyChanged("ShowRounds");
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// Reduces the amount of rounds remaining by 1 to a minimum of 0
        /// </summary>
        public void ReduceRoundsRemaining()
        {
            if (RoundsRemaining > 0)
            {
                RoundsRemaining--;
            }
            else
            {
                // There are no rounds remaining - so de select it
                WmsFakeDb.Database.Remove(this);
            }
        }
    }
}
