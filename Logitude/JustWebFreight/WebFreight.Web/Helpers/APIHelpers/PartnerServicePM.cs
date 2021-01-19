using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class PartnerServicePM
    {
        [Key]
        public int Tenant { get; set; }
        public string AddressId { get; set; }
        public string ContactId { get; set; }
        public string PartnerId { get; set; }
        public string PartnerTypeId { get; set; }
        public bool IsAddressDirty { get; set; }
        public bool IsContactDirty { get; set; }
        public bool IsPartnerDirty { get; set; }
        public AddressPM Address { get; set; }
        public ContactPM Contact { get; set; }
        public AgentPM Agent { get; set; }
        public CustomerPM Customer { get; set; }
        public CustomAgentPM CustomAgent { get; set; }
        public ShippingAgentPM ShippingAgent { get; set; }
        public VendorPM Vendor { get; set; }
        public WarehousePM Warehouse { get; set; }
        public AirlinePM Airline { get; set; }
        public ShippingLinePM ShippingLine { get; set; }
        public TruckerPM Trucker { get; set; }
        public AccountingPartnerPM AccountingPartner { get; set; }
        public bool IsReactivatingContact { get; set; }
        public bool IsConnectingInactiveContact { get; set; }
        public string InactiveContactId { get; set; }

    }
}