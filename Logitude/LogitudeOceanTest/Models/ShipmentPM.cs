using System;
using System.Collections.Generic;

namespace Logitude.OceanTest.Models
{
    public class ShipmentPM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentNumber { get; set; }
        public string CustomerId { get; set; }
        public string MasterShipmentDataId { get; set; }
        public string MasterShipmentNumber { get; set; }
        public string NewConcurrencyGUID { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string BranchId { get; set; }
        public string DepartmentId { get; set; }
        public string FreightPrepaidCollectId { get; set; }
        public string OtherPrepaidCollectId { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string MainCarriageToPortId { get; set; }
        public string MainCarriageFromPortId { get; set; }
        public int? PackagesQuantity { get; set; }
        public string ConcurrencyGUID { get; set; }
        public DateTime CreateDateTime { get; set; }//
        public string StatusId { get; set; }
        public int ShipmentPickUpIndex { get; set; }
        public int ShipmentDeliveryIndex { get; set; }
        public string AgentAddressCountryCode { get; set; }
        public string AgentAddressId { get; set; }
        public string AgentName { get; set; }
        public string AgentId { get; set; }
        public string AgentComputed { get; set; }
        public List<PackagePM> ShipmentPackages { get; set; }
       
        public string ShipmentReceivableStatusName { get; set; }
        public string ShipmentReceivableStatusCode { get; set; }
        public double? GrossWeight { get; set; }
    }
}