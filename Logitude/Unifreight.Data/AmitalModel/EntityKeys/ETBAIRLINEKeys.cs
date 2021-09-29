using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class ETBAIRLINEKeys : EntityKeyFields
    {
        public string AIRLINEID { get; set; }

        public override string GetFullKey()
        {
            return AIRLINEID;
        }

        public override string GetEntityPMName()
        {
            return "ETBAIRLINE";
        }

    }
}


