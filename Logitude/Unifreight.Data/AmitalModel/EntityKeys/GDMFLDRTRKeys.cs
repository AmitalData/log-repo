using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public  class GDMFLDRTRKeys : EntityKeyFields
    {
        public string FOLDERCODE { get; set; }
        

        public override string GetFullKey()
        {
            return FOLDERCODE;
        }

        public override string GetEntityPMName()
        {
            return "GDMFLDRTR";
        }

    }
}
