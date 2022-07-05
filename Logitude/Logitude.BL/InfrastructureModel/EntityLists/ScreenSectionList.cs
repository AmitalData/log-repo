using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
 public   class ScreenSectionList
    {

        public int Tenant { get; set; }
        public string Name { get; set; }
        public string CreatedByUserId { get; set; }
        public string ScreenCode { get; set; }
        public int Number { get; set; }

        public int NumberOfRows { get; set; }
        public bool Inactive { get; set; }


    }
}
