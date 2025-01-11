using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class InvalidEmailResetPassword
    {
        [Key]
        public string Id { get; set; }
        public string IP { get; set; }
        public DateTime CreateDate { get; set; }
        public string Email { get; set; }
    }
}
