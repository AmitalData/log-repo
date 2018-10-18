using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GDMREFKeys : EntityKeyFields
    {
        public string COMID { get; set; }
        public string REFID { get; set; }
        public string REFERENCE { get; set; }

        public override string GetFullKey()
        {
            return COMID + "_" + REFID + "_" + REFERENCE;
        }

        public override string GetEntityPMName()
        {
            return "GDMREF";
        }

    }
}

