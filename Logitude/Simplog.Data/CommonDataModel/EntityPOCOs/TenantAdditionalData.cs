using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ServiceModel.DomainServices;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class TenantAdditionalData
    {
        [Key]
        public int Id { get; set; }
        public int Tenant { get; set; }
        public string DropBoxAccessToken { get; set; }
        public string DropBoxState { get; set; }
        public string DropBoxUID { get; set; }
        public string DropBoxUEmail { get; set; }
        public string PaymentGatewayPartnerCode { get; set; }
        public string PaymentGatewayConnectionString { get; set; }

        public PaymentGatewayPartners PaymentGatewayPartner { get; set; }

    }
}