using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public  class SystemTableRequestParams : RequestParamsBase
    {
        public bool UpdateAllTables { get; set; }
        public string TableId { get; set; }
        public bool  AsTableData { get; set; }

        public bool Pseudo { get; set; }
        public bool ForAnatSaveAsDATASET { get; set; }
    }
}
