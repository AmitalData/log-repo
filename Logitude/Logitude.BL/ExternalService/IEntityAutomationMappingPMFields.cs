using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ExternalService
{
    public interface IEntityAutomationMappingPMFields
    {
        void Map<T1,T2>(T1 poco,T2 oldentityPm);
    }
}

