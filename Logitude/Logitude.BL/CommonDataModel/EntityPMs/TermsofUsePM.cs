using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class TermsofUsePM
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int VersionNumber { get; set; }
        public int Tenant { get; set; }
        public string VersionDocumentId { get; set; }
        public string VersionDocumentName { get; set; }
        public byte[] FileData { get; set; }

        public string PrivateLabelId { get; set; }
        public bool IsNew { get; set; }
    }
}