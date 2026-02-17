using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class TermsofUseSignaturePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime SignedDatetime { get; set; }
        public string ContactId { get; set; }
        public int TermsofUseVersion { get; set; }

    }
}