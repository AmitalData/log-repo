using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBPARTKeys : EntityKeyFields
    {
        public string PARTIALITYID { get; set; }

        public override string GetFullKey()
        {
            return PARTIALITYID;
        }

        public override string GetEntityPMName()
        {
            return "CTBPART";
        }

    }
}

