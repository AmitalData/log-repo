using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBLOADKeys : EntityKeyFields
    {
        public string LPORTID { get; set; }

        public override string GetFullKey()
        {
            return LPORTID;
        }

        public override string GetEntityPMName()
        {
            return "CTBLOAD";
        }

    }
}

