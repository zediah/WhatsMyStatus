using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsMyStatus_DnD_.ViewModels.Core;

namespace WhatsMyStatus_DnD_.ViewModels
{
    public class WmsCombat : WmsPrimaryObject, INotifyPropertyChanged, IChildRelation
    {

        private int _currentHp;

        /// <summary>
        /// The current hp when this combat was resolved after the change in hp
        /// </summary>
        public int CurrentHp
        {
            get { return _currentHp; }
            set
            {
                if (_currentHp != value)
                {
                    _currentHp = value;
                    OnPropertyChanged("CurrentHp");
                }
            }
        }

        private int _changeInHp;

        /// <summary>
        /// The number the hp changes due to this 'combat'
        /// </summary>
        public int ChangeInHp
        {
            get { return _changeInHp; }
            set
            {
                if (_changeInHp != value)
                {
                    _changeInHp = value;
                    OnPropertyChanged("ChangeInHp");
                }
            }
        }

        private HpChangeReasons _changeReason;

        /// <summary>
        ///  The reason the hp was changed
        /// </summary>
        public HpChangeReasons ChangeReason
        {
            get { return _changeReason; }
            set
            {
                if (_changeReason != value)
                {
                    _changeReason = value;
                    OnPropertyChanged("ChangeReason");
                }
            }
        }

        private WmsCharacter _character;

        /// <summary>
        /// The number the hp changes due to this 'combat'
        /// </summary>
        public WmsCharacter Character
        {
            get { return _character; }
            set
            {
                if (_character != value)
                {
                    _character = value;
                    OnPropertyChanged("Character");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddToParent()
        {
            GetParent<WmsCharacter>().AddChildRecord(this);
        }

        public T GetParent<T>() where T : WmsPrimaryObject
        {
            return WmsFakeDb.Database.GetRelatedTable<T>().FirstOrDefault(x => x.Dbseqnum == Character.Dbseqnum);
        }
    }

    public enum HpChangeReasons
    {
        Damage = 1,
        Heal = 2,
        TempHp = 3,
        BonusHeal = 4
    }
}
