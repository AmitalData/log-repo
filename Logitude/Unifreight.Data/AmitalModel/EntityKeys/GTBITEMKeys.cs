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
    public class GTBITEMKeys : EntityKeyFields
    {
        public string PARTNERID { get; set; }
        public string ITEMID { get; set; }

        public override string GetFullKey()
        {
            return PARTNERID.ToString() + '_' + ITEMID.ToString();
        }

        public override string GetEntityPMName()
        {
            return "GTBITEM";
        }

    }

}
