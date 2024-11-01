using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
   public class SessionPolicy
    {
       [Key]
        public string Id { get; set; }
        public int WebTokenLifeTimeInMinutes { get; set; }
        public int WebTokenExpirationWarningInMinutes { get; set; }
    }
}
