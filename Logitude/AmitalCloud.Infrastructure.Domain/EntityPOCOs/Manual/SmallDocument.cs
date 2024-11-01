using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class SmallDocument
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Content { get; set; }

        //public List<Document> Documents { get; set; }
    }

}
