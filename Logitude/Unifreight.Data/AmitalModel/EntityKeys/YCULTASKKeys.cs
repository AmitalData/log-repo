using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class YCULTASKKeys : EntityKeyFields
    {
        public string TASKID { get; set; }
        public int? Tenant { get; set; }

        public override string GetFullKey()
        {
            return TASKID;
        }

        public override string GetEntityPMName()
        {
            return "YCULTASK";
        }

    }
}
