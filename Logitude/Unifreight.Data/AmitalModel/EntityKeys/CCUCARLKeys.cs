using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CCUCARLKeys : EntityKeyFields
    {
        public int FILENO { get; set; }
        public int LINENO { get; set; }
        public int COUNTER { get; set; }
        public int TENANT { get; set; }

        public override string GetFullKey()
        {
            return FILENO.ToString() + '_' + LINENO.ToString() + '_' + COUNTER.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CCUCARL";
        }

    }

}