using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class DefaultAndConfigurationPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public string SearchFields { get; set; }
        public bool? Is_Active { get; set; }
        public bool? StoreInCache { get; set; }
        public string SetKey { get; set; }
        public string AdditionalKey { get; set; }
        public int SortOrder { get; set; }
        public string SetValueType1 { get; set; }
        public string Value1 { get; set; }
        public string SetValueType2 { get; set; }
        public string Value2 { get; set; }
        public bool? AllowInheritance { get; set; }

    }
}
