using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CFIMSVSTATLKeys : EntityKeyFields
    {

        public string GUID { get; set; }

        public override string GetFullKey()
        {
            return GUID;
        }

        public override string GetEntityPMName()
        {
            return "CFIMSVSTATL";
        }

    }
}


