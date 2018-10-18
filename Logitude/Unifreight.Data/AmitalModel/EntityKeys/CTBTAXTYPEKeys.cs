using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBTAXTYPEKeys : EntityKeyFields
    {
        public string TAXTYPE { get; set; }

        public override string GetFullKey()
        {
            return TAXTYPE;
        }

        public override string GetEntityPMName()
        {
            return "CTBTAXTYPE";
        }

    }
}


