using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CTBCOUNTRYKeys : EntityKeyFields
    {
        public string COUNTRYID { get; set; }


        public override string GetFullKey()
        {
            return COUNTRYID;
        }

        public override string GetEntityPMName()
        {
            return "CTBCOUNTRY";
        }

    }
}

