using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GTRTRANKeys : EntityKeyFields
    {
        public string PARTNERID { get; set; }
        public string TABLEID { get; set; }
        public string PARTNERCODE { get; set; }
        public string LOCALCODE { get; set; }

       public override string GetFullKey()
        {
            return PARTNERID + '_' + TABLEID + '_' + PARTNERCODE + '_' + LOCALCODE;
        }

        public override string GetEntityPMName()
        {
            return "GTRTRAN";
        }

    }

    public class GTRTRANParentKeys : EntityKeyFields
    {
        public string PARTNERID { get; set; }
        public string TABLEID { get; set; }
        
        public override string GetFullKey()
        {
            return PARTNERID + '_' + TABLEID ;
        }

        public override string GetEntityPMName()
        {
            return "GTRTRANParent";
        }

    }
}
