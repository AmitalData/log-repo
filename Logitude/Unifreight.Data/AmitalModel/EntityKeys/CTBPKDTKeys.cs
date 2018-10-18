using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBPKDTKeys : EntityKeyFields
    {
        public string PACKDETAIL { get; set; }

        public override string GetFullKey()
        {
            return PACKDETAIL;
        }

        public override string GetEntityPMName()
        {
            return "CTBPKDT";
        }

    }
}

