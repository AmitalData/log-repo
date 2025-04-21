using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class FailedTokenLog
    {
        [Key]
        public string Id { get; set; }
        public string IP { get; set; }
        public string Browser { get; set; }
        public string Token { get; set; }
        public DateTime? GMTDateTime { get; set; }

    }
}
