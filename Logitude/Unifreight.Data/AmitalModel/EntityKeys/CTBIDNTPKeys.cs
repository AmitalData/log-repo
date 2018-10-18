using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBIDNTPKeys : EntityKeyFields
    {
        public string IDENTIFITYPE { get; set; }

        public override string GetFullKey()
        {
            return IDENTIFITYPE;
        }

        public override string GetEntityPMName()
        {
            return "CTBIDNTP";
        }

    }
}

