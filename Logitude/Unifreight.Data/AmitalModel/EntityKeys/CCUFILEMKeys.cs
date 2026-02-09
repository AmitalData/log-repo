using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CCUFILEMKeys : EntityKeyFields
    {
        public int FILENO { get; set; }
        public int TENANT { get; set; } 

        public override string GetFullKey()
        {
            return FILENO.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CCUFILEM";
        }

    }

}
