using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class CommunicationAttachmentList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CommunicationLogId { get; set; }
        public string DocumentId { get; set; }
    }
}