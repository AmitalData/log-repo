using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GTBSERLVKeys : EntityKeyFields
    {
        public string SERVLEVELID { get; set; }

        public override string GetFullKey()
        {
            return SERVLEVELID;
        }

        public override string GetEntityPMName()
        {
            return "GTBSERLV";
        }

    }
}


