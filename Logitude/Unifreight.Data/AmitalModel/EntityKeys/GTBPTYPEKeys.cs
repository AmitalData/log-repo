

using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GTBPTYPEKeys : EntityKeyFields
    {
    
    
        public string APPLICATION { get; set; }
        public string PRICETYPE { get; set; }
    
        public override string GetFullKey()
        {
            return APPLICATION + '_' + PRICETYPE ;
        }

        public override string GetEntityPMName()
        {
            return "GTBPTYPE";
        }

    }
}