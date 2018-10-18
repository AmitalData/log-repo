using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBIMPORTKeys : EntityKeyFields
    {
        public string IMPORTERID { get; set; }

        public override string GetFullKey()
        {
            return IMPORTERID;
        }

        public override string GetEntityPMName()
        {
            return "CTBIMPORT";
        }

    }
}

