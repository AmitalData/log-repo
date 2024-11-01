using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class EntityDate
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string SearchFields { get; set; }


    }
}
