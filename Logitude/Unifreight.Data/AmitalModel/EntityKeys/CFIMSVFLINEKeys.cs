using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CFIMSVFLINEKeys : EntityKeyFields
    {
        public long FILENO { get; set; }
        public string COMID { get; set; }
        public int LINENUM { get; set; }

        public override string GetFullKey()
        {
            return FILENO.ToString() + COMID + LINENUM.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CFIMSVFLINE";
        }

    }
}

