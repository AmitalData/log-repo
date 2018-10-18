using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GAQFILEDATAKeys : EntityKeyFields
    {
        public string ENTNAME { get; set; }
        public string PRIMARYNUM { get; set; }
        public string APPQID { get; set; }
        public string PATH { get; set; }
        public string FIELDID { get; set; }
        
        

        public override string GetFullKey()
        {
            return ENTNAME + '_' + PRIMARYNUM + '_' + APPQID + '_' + PATH + '_' + FIELDID;
        }

        public override string GetEntityPMName()
        {
            return "GAQFILEDATA";
        }

    }

}
