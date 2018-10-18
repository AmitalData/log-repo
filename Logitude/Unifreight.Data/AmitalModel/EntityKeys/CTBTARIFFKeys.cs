using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBTARIFFKeys : EntityKeyFields
    {
        public string TARIFFID { get; set; }

        public override string GetFullKey()
        {
            return TARIFFID;
        }

        public override string GetEntityPMName()
        {
            return "CTBTARIFF";
        }

    }
}

