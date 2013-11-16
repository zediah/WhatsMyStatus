using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Shell;
using WhatsMyStatus_DnD_.ViewModels.Core;

namespace WhatsMyStatus_DnD_.ViewModels
{
    public class WmsStatus : WmsPrimaryObject, INotifyPropertyChanged, IParentRelation
    {

        public WmsStatus()
        {
            CharacterStatuses = new ObservableCollection<WmsCharacterStatus>();
        }

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

        private E_StatusEndingCondition _statusEndingCondition;

        public E_StatusEndingCondition StatusEndingCondition
        {
            get { return _statusEndingCondition; }
            set
            {
                if (_statusEndingCondition != value)
                {
                    _statusEndingCondition = value;
                    NotifyPropertyChanged("StatusEndingCondition");
                }
            }
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

        public ObservableCollection<WmsCharacterStatus> CharacterStatuses { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void AddChildRecord<T>(T child) where T : WmsPrimaryObject
        {
            var charStatus = child as WmsCharacterStatus;
            if (charStatus != null)
            {
                CharacterStatuses.Add(charStatus);
            }
        }

        public void RemoveChildRecord<T>(T child) where T : WmsPrimaryObject
        {
            var wmsCharStatus = child as WmsCharacterStatus;
            if (wmsCharStatus != null)
            {
                CharacterStatuses.Remove(wmsCharStatus);
            }
        }
    }

    public enum E_StatusEndingCondition
    {
        [Description("None")]
        None = 0,
        [Description("# Rounds")]
        Rounds = 1,
        [Description("Until Saved")]
        UntilSaved = 2
    }

    
}
