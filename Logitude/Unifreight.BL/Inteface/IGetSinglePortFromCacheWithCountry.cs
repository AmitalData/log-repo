using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Unifreight.BL.BL.QuoteOPPortsHelper;

namespace Unifreight.BL.Inteface
{
    interface IGetSinglePortFromCacheWithCountry
    {
        Ports GetSingleFromCacheWithCountry(string portId);
    }
}
