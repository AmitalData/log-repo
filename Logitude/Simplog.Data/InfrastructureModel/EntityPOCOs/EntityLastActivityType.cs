using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class EntityLastActivityType
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }

       // public List<EntityLastActivity> EntityLastActivities { get; set; }
    }
}
