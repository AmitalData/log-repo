using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.Enums
{
    public enum SignMethodByQueueEnum
    {
        None = 0,
        MemorySignQueue,
        HybridDbSignQueue,
        HSMSignQueue,
    }

}
