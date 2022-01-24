using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    public class ProductTypeModificationPM
    {
        [Key]
        public string ProductTypeCode { get; set; }
        [Key]
        public int Tenant { get; set; }
        public bool InActive { get; set; }
        public string QuotationDefaultTemplateId { get; set; }
        public string RoutingRQuoteDefaultTemplateId { get; set; }
        public string CostTariffUse { get; set; }
        public string SaleTariffUse { get; set; }


    }
}