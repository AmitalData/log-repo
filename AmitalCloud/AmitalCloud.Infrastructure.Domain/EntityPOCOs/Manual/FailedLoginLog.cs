using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public  class FailedLoginLog
    {
        [Key]
        public string Id { get; set; }
        public string IP { get; set; }
        public string Browser { get; set; }
        public string Email { get; set; }
        public DateTime? GMTDateTime { get; set; }
        public string UserAgent { get; set; }
        public string Reason { get; set; }
        

    }
}
