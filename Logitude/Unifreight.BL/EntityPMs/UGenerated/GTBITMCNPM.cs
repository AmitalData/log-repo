using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unifreight.BL.EntityPMs
{
    public partial class GTBITMCNPM : EntityPM
    {
        public string PARTNERID { get; set; }
        public string ITEMID { get; set; }
        public string PARTNER2ID { get; set; }
        public string ITEM2ID { get; set; }
    }
}
