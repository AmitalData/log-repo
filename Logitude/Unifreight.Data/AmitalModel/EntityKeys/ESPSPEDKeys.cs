using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class ESPSPEDKeys : EntityKeyFields
    {
        public int SPD_NO { get; set; }

        public override string GetFullKey()
        {
            return SPD_NO.ToString();
        }

        public override string GetEntityPMName()
        {
            return "ESPSPED";
        }

    }
}


