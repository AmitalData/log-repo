using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class DocumentTypeCopyPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string DocumentTypeId { get; set; }
        public int IndexOrder { get; set; }
        public bool IsSelectedByDefault { get; set; }
        public bool InActive { get; set; }
        public bool HasDocumentOutCopy { get; set; }
        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}