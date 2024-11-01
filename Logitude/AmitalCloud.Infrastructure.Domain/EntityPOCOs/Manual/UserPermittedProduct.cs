using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class UserPermittedProduct
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public string UserId { get; set; }

        public string ProductTypeCode { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ProductTypeCode")]
        public virtual ProductType ProductType { get; set; }
    }
}
