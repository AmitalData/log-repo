using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class LFIFILEMKeys : EntityKeyFields
    {
        public int DELIVERYNO { get; set; }

        public override string GetFullKey()
        {
            return DELIVERYNO.ToString();
        }

        public override string GetEntityPMName()
        {
            return "LFIFILEM";
        }

    }
}


