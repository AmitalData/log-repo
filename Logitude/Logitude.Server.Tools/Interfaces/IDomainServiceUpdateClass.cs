using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Interfaces
{
    public interface IDomainServiceUpdateClass<TEntityPM>
    {
        void Insert(TEntityPM entityPM);
        void Update(TEntityPM entityPM);
        void Delete(TEntityPM entityPM);
      
    }
}
