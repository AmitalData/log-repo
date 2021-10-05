using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class ETBPORTKeys : EntityKeyFields
    {
        public string PORTID { get; set; }

        public override string GetFullKey()
        {
            return PORTID;
        }

        public override string GetEntityPMName()
        {
            return "ETBPORT";
        }

    }
}


