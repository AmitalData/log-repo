
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public  class GDMFILEVERKeys : EntityKeyFields
    {
        public string COMID { get; set; }
        public int VERSION { get; set; }

        public override string GetFullKey()
        {
            return COMID + '_' + VERSION;
        }

        public override string GetEntityPMName()
        {
            return "GDMFILEVER";
        }

    }
}
