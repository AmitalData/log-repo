using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GAQTEAMUSRKeys : EntityKeyFields
    {
        public string TEAMID { get; set; }

        public override string GetFullKey()
        {
            return TEAMID;
        }

        public override string GetEntityPMName()
        {
            return "GAQTEAMUSR";
        }

    }

}
