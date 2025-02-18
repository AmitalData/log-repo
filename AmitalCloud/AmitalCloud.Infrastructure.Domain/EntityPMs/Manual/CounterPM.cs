using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class CounterPM
    {
        private Counter counter;

        public CounterPM(Counter counter)
        {
            this.counter = counter;
            this.Id = counter.Id;
            this.Tenant = counter.Tenant;
            this.Code = counter.Code;
            this.Name = counter.Name;
            this.ObjectTableId = counter.ObjectTableId;
            this.ChangedByUserId = counter.ChangedByUserId;
            this.ChangedDate = counter.ChangedDate;

        }

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