using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GTBITEMSMSVKeys : EntityKeyFields
    {
        public string PRATID { get; set; }

        public override string GetFullKey()
        {
            return PRATID;
        }

        public override string GetEntityPMName()
        {
            return "GTBITEMSMSV";
        }

    }
}

