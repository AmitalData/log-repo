using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GDMENTITYKeys : EntityKeyFields
    {
        public string COMID { get; set; }
        public string PRIMARYID { get; set; }
        public string PRIMARYNUM { get; set; }

        public override string GetFullKey()
        {
            return COMID + "_" + PRIMARYID + "_" + PRIMARYNUM;
        }

        public override string GetEntityPMName()
        {
            return "GDMENTITY";
        }

    }
}

