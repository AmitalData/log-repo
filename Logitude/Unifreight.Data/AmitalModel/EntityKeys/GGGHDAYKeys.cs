using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GGGHDAYKeys 
    {
        public DateTime HOLIDAY { get; set; }

        public DateTime GetFullKey()
        {
            return HOLIDAY;
        }

        public string GetEntityPMName()
        {
            return "GGGHDAY";
        }

    }
}


