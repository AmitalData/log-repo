using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class DocumentsFilingMetaDataValuePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string DocumentsFilingId { get; set; }
        public string DocumentsMetaDataTypeId { get; set; }
        public string MetaDataValue { get; set; }
        public string DocumentsMetaDataTypeCode { get; set; }

        public ChangeSetOperation ChangeSetOp { get; set; }
    }
}