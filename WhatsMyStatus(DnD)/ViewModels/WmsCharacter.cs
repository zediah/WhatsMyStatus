using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WhatsMyStatus_DnD_.ViewModels.Core;

namespace WhatsMyStatus_DnD_.ViewModels
{
    public class WmsCharacter : WmsPrimaryObject, INotifyPropertyChanged, IParentRelation
    {
        public WmsCharacter()
        {
            CharacterStatuses = new ObservableCollection<WmsCharacterStatus>();
            Combats = new ObservableCollection<WmsCombat>();
        }
        private string _name;

        /// <summary>
        /// The name of the character
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        // Should make this a reference to an object at some point
        private string _characterClass;

        public string CharacterClass
        {
            get { return _characterClass; }
            set
            {
                if (_characterClass != value)
                {
                    _characterClass = value;
                    OnPropertyChanged("CharacterClass");
                }
            }
        }

        // Should make this a reference to an object at some point
        private E_GameSystems _gameSystem;

        public E_GameSystems GameSystem
        {
            get { return _gameSystem; }
            set
            {
                if (_gameSystem != value)
                {
                    _gameSystem = value;
                    OnPropertyChanged("GameSystem");
                }
            }
        }

        private int _level;
        
        /// <summary>
        /// The level of the character
        /// </summary>
        public int Level
        {
            get { return _level; }
            set
            {
                if (_level != value)
                {
                    _level = value;
                    OnPropertyChanged("Level");
                }
            }
        }

        private int _maxHp;

        /// <summary>
        /// The maximum hp of the character
        /// </summary>
        public int MaxHp
        {
            get { return _maxHp; }
            set
            {
                if (_maxHp != value)
                {
                    _maxHp = value;
                    OnPropertyChanged("MaxHp");
                }
            }
        }

        private int _surges;

        /// <summary>
        /// The number of surges this character has...very 4e idea though, should split this out later
        /// </summary>
        public int Surges
        {
            get { return _surges; }
            set
            {
                if(_surges != value)
                {
                    _surges = value;
                    OnPropertyChanged("Surges");
                }
            }
        }

        public ObservableCollection<WmsCharacterStatus> CharacterStatuses { get; private set; }

        public ObservableCollection<WmsCombat> Combats { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddCombat(HpChangeReasons changeReason,  int changeInHp)
        {
            // Find the -actual- change in hp based on the reason
            switch (changeReason)
            {
                case HpChangeReasons.Damage:
                     changeInHp *= -1;
                    // Try take the damage off temp hp first, always.
                    if (HasTempHp())
                    {
                        var currentTempHp = GetCurrentTempHp();
                        // Need to see if we're going to go below the temp hp or not.
                        if (currentTempHp + changeInHp <= 0)
                        {
                            var newCombat = new WmsCombat()
                                                {
                                                    ChangeInHp = -currentTempHp,
                                                    CurrentHp = 0,
                                                    ChangeReason = HpChangeReasons.TempHp,
                                                    Character = this
                                                };
                            WmsFakeDb.Database.Add(newCombat);
                            changeInHp += currentTempHp;
                        }
                        else
                        {
                            // We didn't eat all our temp hp - no change in actual hp.
                            var newCombat = new WmsCombat()
                                                {
                                                    ChangeInHp = changeInHp,
                                                    ChangeReason = HpChangeReasons.TempHp,
                                                    CurrentHp = currentTempHp + changeInHp,
                                                    Character = this
                                                };
                            WmsFakeDb.Database.Add(newCombat);
                            changeInHp = 0;
                        }
                    }
                    break;
                case HpChangeReasons.TempHp:
                    // Temp hp is applied only as the 'highest' temp hp value
                    // e.g. have 5 temp hp, a further 7 temp hp is applied - your new temp hp is 7. Not 12.
                    // Also, if you have 5 temp hp, and you get 3 more - your new temp hp is 5. Not 3 (or 8).
                    var maxTempHp = Math.Max(changeInHp, GetCurrentTempHp());
                    changeInHp = maxTempHp - GetCurrentTempHp();
                    break;
                case HpChangeReasons.Heal:
                    // You can only heal to a maximum of your current hp.
                    // Thus if the heal would take you over your maximum hp - make it only do the amount up to the max hp.
                    // If we get healed, we get healed from a minimum of 0 and up - not from the negative number and up.
                    if (GetCurrentHp() + changeInHp > MaxHp)
                    {
                        changeInHp = MaxHp - GetCurrentHp();
                    }
                    else if (GetCurrentHp() < 0)
                    {
                        changeInHp -= GetCurrentHp();
                    }
                    break;
            }
            // If after modifications we're actually changing the hp - create a combat record for it.
            if (changeInHp != 0)
            {
                var newHp = changeReason == HpChangeReasons.TempHp
                            ? GetCurrentTempHp() + changeInHp
                            : GetCurrentHp() + changeInHp;
                 
                var newCombat = new WmsCombat()
                                    {
                                        ChangeInHp = changeInHp,
                                        ChangeReason = changeReason,
                                        CurrentHp = newHp,
                                        Character = this
                                    };
                WmsFakeDb.Database.Add(newCombat);
            }
        }

        /// <summary>
        /// Whether or not the current character has temp hp or not 
        /// </summary>
        /// <returns></returns>
        public bool HasTempHp()
        {
            var latestTemp = GetLatestTempHp();
            return latestTemp != null && latestTemp.CurrentHp > 0;
        }

        public WmsCombat GetLatestTempHp()
        {
            return Combats.LastOrDefault(x => x.ChangeReason == HpChangeReasons.TempHp);
        }

        public int GetCurrentHp()
        {
            var last = Combats.LastOrDefault(x => x.ChangeReason != HpChangeReasons.TempHp);
            if (last != null)
            {
                return last.CurrentHp;
            }
            return MaxHp;
        }

        public int GetCurrentTempHp()
        {
            var last = Combats.LastOrDefault(x => x.ChangeReason == HpChangeReasons.TempHp);
            if (last != null)
            {
                return last.CurrentHp;
            }
            return 0;
        }

        public void AddChildRecord<T>(T child) where T : WmsPrimaryObject
        {
            var wmsCombat = child as WmsCombat;
            if (wmsCombat != null)
            {
                Combats.Add(wmsCombat);
            }
        }

        /// <summary>
        /// Spends a single surge if the character has one available to heal
        /// </summary>
        /// <param name="bonus">The bonus given while spending a surge, optional</param>
        public void SpendSurge(int bonus = 0)
        {
            if (Surges > 0)
            {
                Surges--;
                // Round down based on 1/4 of max hp of character. This is the rule.
                AddCombat(HpChangeReasons.Heal, (int)Math.Floor((decimal)(MaxHp / 4)) + bonus);
            }
            else
            {
                MessageBox.Show("Unable to spend a surge.\nYou have none you silly duffa!","Surge Unsuccesful", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Remove a single surge from the character. Cannot go below 0.
        /// </summary>
        public void RemoveSurge()
        {
            if (Surges > 0)
            {
                Surges--;
            }
        }

        /// <summary>
        /// Gets a list of status's that currently afflict this character
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WmsCharacterStatus> GetAfflictedStatuses()
        {
            return WmsFakeDb.Database.GetRelatedTable<WmsCharacterStatus>(x => x.Character == this);
        }

        /// <summary>
        /// Gets all the combat records for this character
        /// </summary>
        /// <returns></returns>
        public IEnumerable<WmsCombat> GetCombats()
        {
            return WmsFakeDb.Database.GetRelatedTable<WmsCombat>(x => x.Character == this);
        }

        /// <summary>
        /// Add a single surge to the character
        /// </summary>
        public void AddSurge()
        {
            Surges++;
        }
    }

    public enum E_GameSystems
    {
        [Description("None")]
        None = 0,
        [Description("DnD 4th Edition")]
        DNDFourth = 1
    }
}
