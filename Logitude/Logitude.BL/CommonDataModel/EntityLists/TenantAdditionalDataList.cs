using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.CommonDataModel.EntityLists
{
    public class TenantAdditionalDataList
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

        //public PaymentGatewayPartner PaymentGatewayPartner { get; set; }
    }
}