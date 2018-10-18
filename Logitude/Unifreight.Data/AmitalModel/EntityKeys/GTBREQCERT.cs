using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GTBREQCERTKeys : EntityKeyFields
    {
        public string REQCERT { get; set; }
        public string ENTITY { get; set; }

        public override string GetFullKey()
        {
            return REQCERT + '_' + ENTITY;
        }

        public override string GetEntityPMName()
        {
            return "GTBREQCERT";
        }
    }
}
