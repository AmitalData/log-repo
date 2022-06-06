using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class MSPSPEDKeys : EntityKeyFields
    {
        public int SPDNO { get; set; }

        public override string GetFullKey()
        {
            return SPDNO.ToString();
        }

        public override string GetEntityPMName()
        {
            return "MSPSPED";
        }

    }
}


