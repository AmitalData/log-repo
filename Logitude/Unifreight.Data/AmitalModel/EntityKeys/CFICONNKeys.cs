using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CFICONNKeys : EntityKeyFields
    {
        public string FILENO { get; set; }
        public long CUSTOMFILE { get; set; }

        public override string GetFullKey()
        {
            return FILENO + "_" + CUSTOMFILE.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CFICONN";
        }

    }
}

