using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class YTBTABLEKeys : EntityKeyFields
    {
        public string CUSTTB { get; set; }
        public string TBCODE { get; set; }

        public override string GetFullKey()
        {
            return CUSTTB + '_' + TBCODE;
        }

        public override string GetEntityPMName()
        {
            return "YTBTABLE";
        }

    }
}


