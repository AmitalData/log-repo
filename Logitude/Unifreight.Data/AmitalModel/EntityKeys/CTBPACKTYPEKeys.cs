using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBPACKTYPEKeys : EntityKeyFields
    {
        public string PACKTYPEID { get; set; }

        public override string GetFullKey()
        {
            return PACKTYPEID;
        }

        public override string GetEntityPMName()
        {
            return "CTBPACKTYPE";
        }

    }
}

