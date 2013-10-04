using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsMyStatus_DnD_.ViewModels.Core
{
    public interface IChildRelation
    {
        void AddToParent();
        T GetParent<T>() where T : WmsPrimaryObject;
    }
}
