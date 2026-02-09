using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CCUSUPITEMSIKeys : EntityKeyFields
    {
        public int FILENO { get; set; }
        public int LINENO { get; set; }
        public int ACCLINENO { get; set; }
        public int? Tenant { get; set; }


        public override string GetFullKey()
        {
            return FILENO.ToString() + '_' + LINENO.ToString() + '_' + ACCLINENO.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CCUSUPITEMSI";
        }

    }

}