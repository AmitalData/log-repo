using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityLists
{
    public class UserLoginLogList
    {
        [Key]
        public string Id { get; set; }
        public string IPSiteUri { get; set; }
        public string IP { get; set; }
        public string Browser { get; set; }
        public DateTime? GMTDateTime { get; set; }
        public DateTime? LocalDateTime { get; set; }

    }
    
}
