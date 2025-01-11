using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityLists
{
    public class QueueMessageList
    {
        [Key]
        public long Id { get; set; }
        public string QueueDefinitionCode { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int Status { get; set; }
        public string MessageBody { get; set; }
        public DateTime NextRunDateTime { get; set; }
        public DateTime? ProcessingDateTime { get; set; }
        public DateTime? CompleteDateTime { get; set; }
        public int RetryNumber { get; set; }
        public int Tenant { get; set; }
        public string HashCode { get; set; }

    }
}
