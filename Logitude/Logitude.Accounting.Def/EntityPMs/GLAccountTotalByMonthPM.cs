using Logitude.Server.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityPMs
{
    public partial class GLAccountTotalByMonthPM : EntityPM
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
            
        }
    }
}
