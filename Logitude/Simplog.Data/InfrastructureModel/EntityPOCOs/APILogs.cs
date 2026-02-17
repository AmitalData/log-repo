using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class APILogs
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Direction { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime LastUpdateDateUTC { get; set; }
        public int NumberOfRetries { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Subject { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public string PartnerName { get; set; }
        public string Refrence { get; set; }
        public string SearchFields { get; set; }
        public string LastExceptionMessage { get; set; }
        public string CorrelationId { get; set; }

        public string BatchNumber { get; set; }
        public string QueueType { get; set; }
        public string QueueMessage { get; set; }

        public string QueueMessageMoreDetailsId { get; set; }

        public string CustomerId { get; set; }

        public APILogsData APILogsData { get; set; }

    }
}
