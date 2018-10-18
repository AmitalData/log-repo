using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityPMs
{
    public partial class ReconcileMethodPM : EntityPM
    {
        public enum ReconcileMethodEnum
        {
            LocalCurrency =0	,//מטבע מקומי
            ForeignCurrency=1//	מטבע חוץ
        }
    }
}
