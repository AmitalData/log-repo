using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBTRANSPKeys : EntityKeyFields
    {
        public string TRANSPTYPE { get; set; }

        public override string GetFullKey()
        {
            return TRANSPTYPE;
        }

        public override string GetEntityPMName()
        {
            return "CTBTRANSP";
        }

    }
}

