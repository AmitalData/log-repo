using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBRGOWNKeys : EntityKeyFields
    {
        public short RIGHTID { get; set; }

        public override string GetFullKey()
        {
            return RIGHTID.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CTBRGOWN";
        }

    }
}

