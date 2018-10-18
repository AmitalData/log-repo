using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class QueryGroup
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public int IndexOrder { get; set; }

        //public List<Query> Queries { get; set; }
    }
}