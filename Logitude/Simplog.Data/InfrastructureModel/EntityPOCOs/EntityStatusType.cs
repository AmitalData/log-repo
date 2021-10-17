using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class EntityStatusType
    {
        [Key]
        public string  Code { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }
    }
}