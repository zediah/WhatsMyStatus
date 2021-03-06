﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsMyStatus_DnD_.ViewModels.Core;

namespace WhatsMyStatus_DnD_.ViewModels
{
    public class WmsCharacterStatus : WmsPrimaryObject, INotifyPropertyChanged, IChildRelation
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

        private readonly WmsLoadObject<WmsStatus> _status = new WmsLoadObject<WmsStatus>();

        /// <summary>
        /// The status this is for
        /// </summary>
        public WmsStatus Status
        {
            get { return _status.Load(); }
            set
            {
               if (_status.Set(value))
               {
                    NotifyPropertyChanged("State");
               }
            }
        }

        private readonly WmsLoadObject<WmsCharacter> _character = new WmsLoadObject<WmsCharacter>();

        /// <summary>
        /// The character this is for
        /// </summary>
        public WmsCharacter Character
        {
            get { return _character.Load(); }
            set
            {
                if (_character.Set(value))
                {
                    NotifyPropertyChanged(("Character"));
                }
            }
        }

        public bool ShowRounds
        {
            get { return Status.StatusEndingCondition == E_StatusEndingCondition.Rounds && AfflictedWithStatus; }
        }

        public bool ShowUntilSaved
        {
            get { return Status.StatusEndingCondition == E_StatusEndingCondition.UntilSaved && AfflictedWithStatus; }
        }

        private bool _afflictedWithStatus;

        /// <summary>
        ///  Whether or not the character is afflicted by this status or not
        /// </summary>
        public bool AfflictedWithStatus
        {
            get { return _afflictedWithStatus; }
            set
            {
                if(_afflictedWithStatus != value)
                {
                    
                    _afflictedWithStatus = value;
                    NotifyPropertyChanged("AfflictedWithStatus");
                    NotifyPropertyChanged("ShowRounds");
                    NotifyPropertyChanged("ShowUntilSaved");
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
            // if we reduce it to 0, turn it off.
            if (RoundsRemaining > 1)
            {
                RoundsRemaining--;
            }
            else
            {
                // There are no rounds remaining - so de select it
                AfflictedWithStatus = false;
            }
        }

        public void AddToParent()
        {
            if (Status != null)
            {
                Status.AddChildRecord(this);
            }
            if (Character != null)
            {
                Character.AddChildRecord(this);
            }
        }

        public T GetParent<T>() where T : WmsPrimaryObject
        {
            throw new NotImplementedException();
        }

        public void RemoveFromParent()
        {
            if (Status != null)
            {
                Status.RemoveChildRecord(this);
            }
            if (Character != null)
            {
                Character.RemoveChildRecord(this);
            }
        }
    }
}
