using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WhatsMyStatus_DnD_.ViewModels.Core;

namespace WhatsMyStatus_DnD_.ViewModels
{
    public sealed class WmsFakeDb
    {
        public WmsFakeDb()
        {
            Characters = new ObservableCollection<WmsCharacter>();
            Statuses = new ObservableCollection<WmsStatus>();
            Combats = new ObservableCollection<WmsCombat>();
            CharacterStatuses = new ObservableCollection<WmsCharacterStatus>();

            CachedTypes = new Dictionary<Type, IList>();
            CachedTypes.Add(typeof(WmsCharacter), Characters);
            CachedTypes.Add(typeof(WmsCombat), Combats);
            CachedTypes.Add(typeof(WmsStatus), Statuses);
            CachedTypes.Add(typeof(WmsCharacterStatus), CharacterStatuses);
        }

        public ObservableCollection<WmsCharacter> Characters { get; private set; }

        public ObservableCollection<WmsStatus> Statuses { get; private set; }

        public ObservableCollection<WmsCharacterStatus> CharacterStatuses { get; private set; }

        public ObservableCollection<WmsCombat> Combats { get; private set; }

        private static readonly Lazy<WmsFakeDb> database =
            new Lazy<WmsFakeDb>(() => new WmsFakeDb());

        public static WmsFakeDb Database { get { return database.Value; } }

        private Dictionary<Type, IList> CachedTypes { get; set; }
        
        public IList<T> GetRelatedTable<T>()  where T : WmsPrimaryObject
        {
            try
            {
                return GetRelatedTable(typeof (T)).Cast<T>().ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }

        public IList GetRelatedTable(Type t)
        {
            try
            {
                if (CachedTypes.ContainsKey(t))
                {
                    return CachedTypes[t];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }

        public void Add<T>(T newObj) where T: WmsPrimaryObject
        {
            try
            {
                if (CachedTypes.ContainsKey(typeof(T)))
                {
                    newObj.SetDbseqnum<T>();
                    if (newObj is IChildRelation)
                    {
                        ((IChildRelation)newObj).AddToParent();
                    }
                    CachedTypes[typeof(T)].Add(newObj);
                    IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                    if (!settings.Contains(typeof(T).FullName))
                    {
                        settings.Add(typeof(T).FullName, new List<T>());
                    }
                    if (!((List<T>)settings[typeof(T).FullName]).Contains(newObj))
                    {
                        ((List<T>) settings[typeof (T).FullName]).Add(newObj);
                    }
                    settings.Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void Remove<T>(T removeObj) where T: WmsPrimaryObject
        {
            try
            {
                if (CachedTypes.ContainsKey(removeObj.GetType()) && 
                    CachedTypes[removeObj.GetType()].Contains(removeObj))
                {
                    CachedTypes[removeObj.GetType()].Remove(removeObj);
                }

                IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
                if (settings.Contains(typeof(T).FullName) && ((List<T>)settings[typeof(T).FullName]).Contains(removeObj))
                {
                    ((List<T>)settings[typeof(T).FullName]).Remove(removeObj);
                }
                settings.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool IsDataLoaded = false;

        public void SetupFromIsolatedStorage()
        {
            LoadFromIsolatedStorage<WmsCharacter>();
            LoadFromIsolatedStorage<WmsCharacterStatus>();
            LoadFromIsolatedStorage<WmsStatus>();
            LoadFromIsolatedStorage<WmsCombat>();
        }

        private void LoadFromIsolatedStorage<T>() where T: WmsPrimaryObject
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            var key = typeof (T).FullName;
            if (settings.Contains(key))
            {
                foreach(var value in (IList<T>)settings[key])
                {
                    Add(value);
                }
            }
        }
    }
}
