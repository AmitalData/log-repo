using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GTBMANDTKeys : EntityKeyFields
    {
        public string CLIENTCODE { get; set; }
        public string FORMNAME { get; set; }
        public string ENTITY { get; set; }
        public string FIELDNAME { get; set; }

        public override string GetFullKey()
        {
            return CLIENTCODE + '_' + FORMNAME + '_' + ENTITY + '_' + FIELDNAME;
        }

        public override string GetEntityPMName()
        {
            return "GTBMANDT";
        }

    }
}