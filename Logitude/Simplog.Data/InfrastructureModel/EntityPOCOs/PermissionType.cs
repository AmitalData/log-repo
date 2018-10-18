using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class PermissionType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }

  //      public List<ObjectField> CustomerObjectFields { get; set; }
      //  public List<ObjectField> AgentObjectFields { get; set; }

    }
}