using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBAPPROVKeys : EntityKeyFields
    {
        public string APPROVCODEID { get; set; }

        public override string GetFullKey()
        {
            return APPROVCODEID;
        }

        public override string GetEntityPMName()
        {
            return "CTBAPPROV";
        }

    }
}

