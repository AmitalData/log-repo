using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GITITEMCRKeys : EntityKeyFields
    {
        public string COUNTER { get; set; }
        public string REQCERT { get; set; }
        

        public override string GetFullKey()
        {
            return COUNTER + '_' + REQCERT;
        }

        public override string GetEntityPMName()
        {
            return "GITITEMCR";
        }

    }

}
