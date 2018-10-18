using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class DWQueryFilterPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DWQueryId { get; set; }
        public string DWObjectFieldId { get; set; }
        public bool IsPredefined { get; set; }
        public string PredefinedValue { get; set; }
        public string PredefinedValue2 { get; set; }
        public string Operator { get; set; }
        public int IndexOrder { get; set; }
        public string UserId { get; set; }
    }
}
