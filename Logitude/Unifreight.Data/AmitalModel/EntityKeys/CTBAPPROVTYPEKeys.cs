using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBAPPROVTYPEKeys : EntityKeyFields
    {
        public string APPROVTYPEID { get; set; }

        public override string GetFullKey()
        {
            return APPROVTYPEID;
        }

        public override string GetEntityPMName()
        {
            return "CTBAPPROVTYPE";
        }

    }
}

