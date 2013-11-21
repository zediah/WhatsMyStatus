using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsMyStatus_DnD_.ViewModels
{
    public class WmsStatusFilter : INotifyPropertyChanged
    {
        public ObservableCollection<E_GameSystems> GameSystemses
        {
            get { return new ObservableCollection<E_GameSystems>(Enum.GetValues(typeof(E_GameSystems)).Cast<E_GameSystems>()); }
        }

        private E_GameSystems? _filterGameSystem = null;

        public E_GameSystems? FilterGameSystem
        {
            get { return _filterGameSystem; }
            set
            {
                if (_filterGameSystem != value)
                {
                    _filterGameSystem = value;
                    OnPropertyChanged("FilterGameSystem");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
