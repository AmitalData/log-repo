using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class UserLoginLog
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string IP { get; set; }
        public string Browser { get; set; }
        public string UserId { get; set; }

        public DateTime? GMTDateTime { get; set; }
        public DateTime? LocalDateTime { get; set; }

        public string ComputerId { get; set; }

        public string UserAgent { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
