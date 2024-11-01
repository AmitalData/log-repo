using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.Interfaces
{
    public interface IDomainServiceUpdateClass<TEntityPM>
    {
        void Insert(TEntityPM entityPM);
        void Update(TEntityPM entityPM);
        void Delete(TEntityPM entityPM);
      
    }
}
