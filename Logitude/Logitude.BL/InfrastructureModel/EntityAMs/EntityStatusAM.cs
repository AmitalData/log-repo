using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityAMs
{
    public class EntityStatusAM
    {
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ObjectTableName { get; set; }
        public int StatusWeight { get; set; }
        public int? StatusLocalWeight { get; set; }
        public string EntityStatusTypeCode { get; set; }
        public bool AllowPartial { get; set; }
        public bool IsDigitalPortal { get; set; }
    }
}
