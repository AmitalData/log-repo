using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBUNLOADKeys : EntityKeyFields
    {
        public string ULPORTID { get; set; }

        public override string GetFullKey()
        {
            return ULPORTID;
        }

        public override string GetEntityPMName()
        {
            return "CTBUNLOAD";
        }

    }
}

