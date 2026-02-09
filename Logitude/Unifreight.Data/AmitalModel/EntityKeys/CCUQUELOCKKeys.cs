using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CCUQUELOCKKeys : EntityKeyFields
    {
        public string ENTNAME { get; set; }
        public string FILENO { get; set; }
        public int? Tenant { get; set; }


        public override string GetFullKey()
        {
            return ENTNAME + '_' + FILENO;
        }

        public override string GetEntityPMName()
        {
            return "CCUQUELOCK";
        }
    }
}
