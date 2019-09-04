using System;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.EntityPMs
{
    [CustomValidation(typeof(Validators.ClassLevelValidator), "ValidateClass")]
    public class TenantAdditionalDataPM
    {
        [Key]
        public int Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string DropBoxAccessToken { get; set; }
        public string DropBoxState { get; set; }
        public string DropBoxUID { get; set; }
        public string DropBoxUEmail { get; set; }
        public string PaymentGatewayPartnerCode { get; set; }
        public string PaymentGatewayConnectionString { get; set; }
           //public PaymentGatewayPartner PaymentGatewayPartner { get; set; }

    }
}
