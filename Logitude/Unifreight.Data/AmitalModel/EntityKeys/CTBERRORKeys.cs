using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBERRORKeys : EntityKeyFields
    {
        public string ERRORCODE { get; set; }

        public override string GetFullKey()
        {
            return ERRORCODE;
        }

        public override string GetEntityPMName()
        {
            return "CTBERROR";
        }

    }
}

