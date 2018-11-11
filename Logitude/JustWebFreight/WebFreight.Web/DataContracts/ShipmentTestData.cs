using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    public class ShipmentTestData
    {
        public string CountryId { get; set; }
        public string Port1Id { get; set; }
        public string Port2Id { get; set; }
        public string Port1CountryId { get; set; }
        public string Port2CountryId { get; set; }
        public string CurrencyId { get; set; }
        public string UserId { get; set; }
        public string BranchId { get; set; }
        public string DepartmentId { get; set; }

        public string ShipperId { get; set; }
        public string ShipperAddressId { get; set; }

        public string CustomerId { get; set; }
        public string CustomerAddressId { get; set; }

        public string ConsigneeId { get; set; }
        public string ConsigneeAddressId { get; set; }

        public string AgentId { get; set; }
        public string AgentAddressId { get; set; }

        public string IncotermId { get; set; }
        public string AirlineId { get; set; }
        public string TruckerId { get; set; }
        public string ShippingLineId { get; set; }
        public string AirChargesTypeId { get; set; }
        public string OceanChargesTypeId { get; set; }
        public string InlandChargesTypeId { get; set; }
        public string OtherTenantShipmentId { get; set; }
        public string Notify1Id { get; set; }
        public string Notify1AddressId { get;  set; }
    }
}