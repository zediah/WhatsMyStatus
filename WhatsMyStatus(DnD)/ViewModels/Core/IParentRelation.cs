using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsMyStatus_DnD_.ViewModels.Core
{
    public interface IParentRelation
    {
        void AddChildRecord<T>(T child) where T: WmsPrimaryObject;
        void RemoveChildRecord<T>(T child) where T: WmsPrimaryObject;
    }
}
