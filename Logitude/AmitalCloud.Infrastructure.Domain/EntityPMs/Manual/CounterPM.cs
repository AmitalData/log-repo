using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class CounterPM
    {
        [Key]
        public string Id { get; set; }

        public int Tenant { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string ObjectTableId { get; set; }

        public string ChangedByUserId { get; set; }
        public DateTime? ChangedDate { get; set; }

    }
}