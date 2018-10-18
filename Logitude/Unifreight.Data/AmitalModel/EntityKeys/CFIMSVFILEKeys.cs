using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CFIMSVFILEKeys : EntityKeyFields
    {
        public long FILENO { get; set; }
        public string COMID { get; set; }

        public override string GetFullKey()
        {
            return FILENO.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CFIMSVFILE";
        }

    }
}

