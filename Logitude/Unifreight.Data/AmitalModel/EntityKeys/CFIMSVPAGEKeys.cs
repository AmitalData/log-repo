using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CFIMSVPAGEKeys : EntityKeyFields
    {
        public long FILENO { get; set; }
        public string COMID { get; set; }
        public int PAGENUM { get; set; }
        public string QUETYPE { get; set; }

        public override string GetFullKey()
        {
            return FILENO.ToString() + '_' + COMID.ToString() + '_' + PAGENUM.ToString() + '_' + QUETYPE.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CFIMSVPAGE";
        }

    }
}

