using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GDFDATAKeys : EntityKeyFields
    {
        public string DISTRID { get; set; }
        public string DEFID { get; set; }
        public string BRANCHID { get; set; }
        public string CARDID { get; set; }

       public override string GetFullKey()
        {
            return DISTRID + '_' + DEFID + '_' + BRANCHID + '_' + CARDID;
        }

        public override string GetEntityPMName()
        {
            return "GDFDATA";
        }

    }

}
