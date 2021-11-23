using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class ETBPAYTRKeys : EntityKeyFields
    {
        public string PTERMID { get; set; }

        public override string GetFullKey()
        {
            return PTERMID;
        }

        public override string GetEntityPMName()
        {
            return "ETBPAYTR";
        }

    }
}


