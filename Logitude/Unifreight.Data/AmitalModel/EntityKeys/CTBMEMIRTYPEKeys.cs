using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBMEMIRTYPEKeys : EntityKeyFields
    {
        public int? MEMIRTYPE { get; set; }

        public override string GetFullKey()
        {
            return MEMIRTYPE.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CTBMEMIRTYPE";
        }

    }
}

