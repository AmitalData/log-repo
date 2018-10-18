using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GTBDOCKeys : EntityKeyFields
    {
        public string DOCID { get; set; }

        public override string GetFullKey()
        {
            return DOCID;
        }

        public override string GetEntityPMName()
        {
            return "GTBDOC";
        }

    }
}

