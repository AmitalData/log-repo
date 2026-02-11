using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GGGQCKeys : EntityKeyFields
    {
        public string QUEID { get; set; }
        public int? Tenant { get; set; }

        public override string GetFullKey()
        {
            return QUEID;
        }

        public override string GetEntityPMName()
        {
            return "GGGQC";
        }

    }

}
