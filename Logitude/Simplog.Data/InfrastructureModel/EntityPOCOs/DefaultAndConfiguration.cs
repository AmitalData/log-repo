using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class DefaultAndConfiguration
    {
        [Key]
        public string Id { get; set; }
        public string QueueDefinitionCode { get; set; }
        public string CreateDateTime { get; set; }
        public string SearchFields { get; set; }
        public Boolean? Is_Active { get; set; }
        public Boolean? StoreInCache { get; set; }
        public string SetKey { get; set; }
        public string AdditionalKey { get; set; }
        public int SortOrder { get; set; }
        public string SetValueType1 { get; set; }
        public string Value1 { get; set; }
        public string SetValueType2 { get; set; }
        public string Value2 { get; set; }
        public Boolean? AllowInheritance { get; set; }


    }
}
