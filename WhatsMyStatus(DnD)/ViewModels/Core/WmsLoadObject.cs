using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsMyStatus_DnD_.ViewModels.Core
{
    public class WmsLoadObject<T> where T : WmsPrimaryObject
    {
        private int _dbseqnum;
        
        public int Dbseqnum
        {
            get { return _dbseqnum; }
            set { _dbseqnum = value; }
        }

        private T _primaryObject = null;

        public T PrimaryObject
        {
            get
            {
                if (_primaryObject == null && Dbseqnum > 0)
                {
                    // There should only be one though...
                   _primaryObject = WmsFakeDb.Database.GetRelatedTable<T>(x => x.Dbseqnum == Dbseqnum).FirstOrDefault();
                }
                return _primaryObject;
            }
        }

        public T Load()
        {
            return PrimaryObject;
        }

        public bool Set(T value)
        {
            // If we're passed through null, clear everything
            if (value == null)
            {
                // clear the load reference
                _primaryObject = null;
                Dbseqnum = 0;
                return true;
            }

            // If we're passed through a value, make sure it's not the same dbseqnum
            if (value.Dbseqnum != Dbseqnum)
            {
                Dbseqnum = value.Dbseqnum;
                _primaryObject = null;
                return true;
            }
            return false;
        }
    }
}
