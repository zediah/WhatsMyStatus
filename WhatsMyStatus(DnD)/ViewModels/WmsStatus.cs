using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsMyStatus_DnD_.ViewModels.Core;

namespace WhatsMyStatus_DnD_.ViewModels
{
    public class WmsStatus : WmsPrimaryObject, INotifyPropertyChanged
    {
        private string _name = "";

        /// <summary>
        /// The name of the status itself
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _effect = "";

        /// <summary>
        /// The effect that the status will have in writing.
        /// </summary>
        public string Effect
        {
            get { return _effect; }
            set
            {
                if (_effect != value)
                {
                    _effect = value;
                    NotifyPropertyChanged("Effect");
                }
            }

        }

        private bool _roundsRequired = false;

        /// <summary>
        /// Whether a rounds count is required for this status
        /// </summary>
        public bool RoundsRequired
        {
            get { return _roundsRequired; }
            set { if (_roundsRequired != value)
            {
                _roundsRequired = value;
                NotifyPropertyChanged("RoundsRequired");
            }}
        }

        /// <summary>
        /// The game system this status is applicable too
        /// </summary>
        private E_GameSystems _gameSystem;

        public E_GameSystems GameSystem
        {
            get { return _gameSystem; }
            set
            {
                if (_gameSystem != value)
                {
                    _gameSystem = value;
                    NotifyPropertyChanged("GameSystem");
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
    }
}
