using System;
using System.Collections.Generic;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CCUTRANSPVALKeys : EntityKeyFields
    {
        public int FILENO { get; set; }
        public int LINENO { get; set; }

        public override string GetFullKey()
        {
            return FILENO.ToString() + '_' + LINENO.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CCUTRANSPVAL";
        }

    }

}