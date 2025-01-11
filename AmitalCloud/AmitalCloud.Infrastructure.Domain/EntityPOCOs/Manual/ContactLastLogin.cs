using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ContactLastLogin
    {
        [Key]
        public string Id { get; set; }
        public string ComputerId { get; set; }
        public DateTime? LoginDateTime { get; set; }
        public int Tenant { get; set; }


        public virtual Contact Contact { get; set; }

    }
}
