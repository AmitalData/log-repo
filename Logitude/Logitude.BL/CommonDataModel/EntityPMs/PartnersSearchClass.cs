using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class PartnersSearchClass
    {
        [Key]
        public string Id { get; set; }
        public string PartnerId { get; set; }
        public string PartnerType { get; set; }        
        public string PartnerName { get; set; }
        public string SalesmanUserId { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerCity { get; set; }
        public string PartnerCountry { get; set; }
    }
}
