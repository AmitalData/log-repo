using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
   public class DWQueryPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        //public string DWObjectTableCode { get; set; }
        public string SQLString { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdateByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
