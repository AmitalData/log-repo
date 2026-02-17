using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class SharedLogisticDocumentPM
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string DocumentId { get; set; }
        public string DocumentTypeCode { get; set; }
        public string Url { get; set; }
        public string FileExtension { get; set; }
        public string FileName { get; set; }
        public string Reference { get; set; }
        public bool IsDigitallySigned { get; set; }
    }
}