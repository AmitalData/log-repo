using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class EFIMMNKeys : EntityKeyFields
    {
        public long FILENO { get; set; }
        public int STORGENO { get; set; }


        public override string GetFullKey()
        {
            return FILENO.ToString();
        }

        public override string GetEntityPMName()
        {
            return "EFIMMN";
        }

    }
}


