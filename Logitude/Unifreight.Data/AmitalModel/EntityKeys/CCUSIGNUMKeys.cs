using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CCUSIGNUMKeys : EntityKeyFields
    {
        public int FILENO { get; set; }
        public int LINENOMSHGR { get; set; }
        public int LINENOSIGN { get; set; }
        public int? Tenant { get; set; }


        public override string GetFullKey()
        {
            return FILENO.ToString() + '_' + LINENOMSHGR.ToString() + '_' + LINENOSIGN.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CCUSIGNUM";
        }

    }

}