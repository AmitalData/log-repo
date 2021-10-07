using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{

    public class DWObjectTablePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string TypeCode { get; set; }
        public bool IsClosed { get; set; }
        public string DefaultFilterBy { get; set; }
        public string DataViewName { get; set; }
        public bool HasPivotColumn { get; set; }
        public string PivotFieldCode { get; set; }
        public string AdditionalFactCode { get; set; }
        public string AdditionalFactForeignKey { get; set; }
        public string ParentFactCode { get; set; }
        public string RecordType { get; set; }
        public string DisplayName { get; set; }

        public string ObjectTableName { get; set; }
        public int MaxNumberOfCustomFields { get; set; }
        public bool HasCustomFields { get; set; }
        public string AdditionalFactRelationType { get; set; }
        public string AdditionalConditions { get; set; }

        
    }
}
