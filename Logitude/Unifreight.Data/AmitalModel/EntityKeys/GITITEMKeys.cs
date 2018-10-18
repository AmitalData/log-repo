using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GITITEMKeys : EntityKeyFields
    {
        public string COUNTER { get; set; }

        public override string GetFullKey()
        {
            return COUNTER.ToString();
        }

        public override string GetEntityPMName()
        {
            return "GITITEM";
        }

    }
}

