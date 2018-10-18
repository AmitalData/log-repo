using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBRESHTYPEKeys : EntityKeyFields
    {
        public string RESHIMONTYPE { get; set; }

        public override string GetFullKey()
        {
            return RESHIMONTYPE;
        }

        public override string GetEntityPMName()
        {
            return "CTBRESHTYPE";
        }

    }
}