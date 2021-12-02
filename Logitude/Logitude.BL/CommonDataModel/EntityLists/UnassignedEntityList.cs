using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class UnassignedEntityList
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string UnassignedCode { get; set; }
    }
}
