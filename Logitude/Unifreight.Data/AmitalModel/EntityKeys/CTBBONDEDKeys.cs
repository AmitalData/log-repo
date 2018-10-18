using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBBONDEDKeys : EntityKeyFields
    {
        public string WAREHOUSEID { get; set; }

        public override string GetFullKey()
        {
            return WAREHOUSEID;
        }

        public override string GetEntityPMName()
        {
            return "CTBBONDED";
        }

    }
}

