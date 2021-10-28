using System;
using System.ComponentModel.DataAnnotations;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class MultiEntityUpdateLog
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CreatedByUserId { get; set; }
        public string StatusCode { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public string XMLData { get; set; }
        public string ObjectTableId { get; set; }
        public int RetryNumber { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
