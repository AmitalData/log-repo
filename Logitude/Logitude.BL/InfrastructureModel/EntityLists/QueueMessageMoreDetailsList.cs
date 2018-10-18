using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityLists
{
    public class QueueMessageMoreDetailsList
    {
        [Key]
        public long  Id { get; set; }
        public string QueueDefinitionCode { get; set; }
        public DateTime CreateDateTime { get; set; }
        public int Status { get; set; }
        public string MessageBody { get; set; }
        public DateTime NextRunDateTime { get; set; }
        public DateTime? ProcessingDateTime { get; set; }
        public DateTime? CompleteDateTime { get; set; }
        public int RetryNumber { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string StatusName { get; set; }

    }
}
