using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class ATBPTILKeys : EntityKeyFields
    {
        public string BRANID { get; set; }

        public override string GetFullKey()
        {
            return BRANID;
        }

        public override string GetEntityPMName()
        {
            return "ATBPTIL";
        }

    }
}

