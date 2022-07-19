using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GTBFUSTATUKeys : EntityKeyFields
    {
        public string ENTNAME { get; set; }
        public string STATUSCODE { get; set; }


        public override string GetFullKey()
        {
            return ENTNAME;
        }

        public override string GetEntityPMName()
        {
            return "GTBFUSTATU";
        }

    }
}


