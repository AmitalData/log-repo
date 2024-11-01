using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.Interfaces
{
    public interface IBaseEntityQueryService<TEntityPM> where TEntityPM : IEntityPM, new()
    {
        TEntityPM GetSingle(IEnumerable<KeyValuePair<string, string>> paramList, bool getComposition, bool getFromCache);
    }
}
