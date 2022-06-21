using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
 public   class ScreenSectionList
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string CreateByUserId { get; set; }
        public string ScreenCode { get; set; }
    }
}
