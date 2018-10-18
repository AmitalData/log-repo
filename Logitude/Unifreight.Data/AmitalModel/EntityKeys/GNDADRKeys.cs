using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.Data.AmitalModel.EntityKeys
{
    public class GNDADRKeys : EntityKeyFields
    {
        public string CARDID { get; set; }
        public int LINE { get; set; }

       public override string GetFullKey()
        {
            return CARDID + '_' + LINE.ToString();
        }

        public override string GetEntityPMName()
        {
            return "GNDADR";
        }

    }

    //public class GNDADRParentKeys : EntityKeyFields
    //{
    //    public string CARDID { get; set; }
        
    //    public override string GetFullKey()
    //    {
    //        return CARDID;
    //    }

    //    public override string GetEntityPMName()
    //    {
    //        return "GNDADRParent";
    //    }

    //}
}