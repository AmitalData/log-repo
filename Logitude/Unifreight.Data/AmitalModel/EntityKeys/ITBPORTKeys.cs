using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class ITBPORTKeys : EntityKeyFields
    {
        public string PORTID { get; set; }

        public override string GetFullKey()
        {
            return PORTID;
        }

        public override string GetEntityPMName()
        {
            return "ITBPORT";
        }

    }
}


