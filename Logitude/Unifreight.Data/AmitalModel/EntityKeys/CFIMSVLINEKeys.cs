using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CFIMSVLINEKeys : EntityKeyFields
    {
        public long FILENO { get; set; }
        public string COMID { get; set; }
        public int PAGENUM { get; set; }
        public int LINENUM { get; set; }

        public override string GetFullKey()
        {
            return FILENO.ToString() + '_' + COMID + '_' + PAGENUM.ToString() + '_' + LINENUM.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CFIMSVLINE";
        }

    }
}

