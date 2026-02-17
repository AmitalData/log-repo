using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.GlobalModel.EntityLists
{
    public class AnalyzeQueueList
    {
        [Key]
        public string Id { get; set; }
        public byte[] MessageBody { get; set; }
        public int Tenant { get; set; }
        public string From { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public double FileSize { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public string CommunicationLogId { get; set; }
        public string Subject { get; set; }
        public bool ConnectedToTenant { get; set; }
        public bool ConnectedToEntity { get; set; }
        public int Retries { get; set; }
        public string EntityReference {get; set;}
        public string ObjectTableName { get; set; }
        public string SearchFields { get; set; }
        public string StackTrace { get; set; }
        public string TenantName { get; set; }
        public string AWBNumber { get; set; }
        public string AckReason { get; set; }
        public string Log { get; set; }

    }
}