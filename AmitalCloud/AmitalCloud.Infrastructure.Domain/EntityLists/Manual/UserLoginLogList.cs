using System;
using System.ComponentModel.DataAnnotations;

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
