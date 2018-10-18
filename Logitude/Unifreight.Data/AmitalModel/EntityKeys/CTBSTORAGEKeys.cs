using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBSTORAGEKeys : EntityKeyFields
    {
        public string STORAGESITE { get; set; }

        public override string GetFullKey()
        {
            return STORAGESITE;
        }

        public override string GetEntityPMName()
        {
            return "CTBSTORAGE";
        }

    }
}

