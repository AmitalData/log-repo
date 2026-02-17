using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class CCUCRREQKeys : EntityKeyFields
    {
        public string ENTNAME { get; set; }
        public int FILENO { get; set; }
        public int ACCLINENO { get; set; }
        public int ITEMLINE { get; set; }
        public int LINENO { get; set; }

        public override string GetFullKey()
        {
            return ENTNAME + '_' + FILENO.ToString() + '_' + ACCLINENO.ToString() + '_' + ITEMLINE.ToString() + '_' + LINENO.ToString();
        }

        public override string GetEntityPMName()
        {
            return "CCUCRREQ";
        }

    }

}