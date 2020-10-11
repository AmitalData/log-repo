using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CFIMSVREMKeys : EntityKeyFields
    {
        public long FILENO { get; set; }
        public string COMID { get; set; }
        public int PAGENUM { get; set; }
        public int TOP { get; set; }
        public int LEFT { get; set; }
        public int HEIGHT { get; set; }
        public int WIDTH { get; set; }

        public override string GetFullKey()
        {
            return FILENO.ToString() + '_' + COMID + '_' + PAGENUM.ToString() + '_' + TOP.ToString() + '_' + LEFT.ToString() + '_' + HEIGHT.ToString() + '_' + WIDTH.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CFIMSVREM";
        }

    }
}

