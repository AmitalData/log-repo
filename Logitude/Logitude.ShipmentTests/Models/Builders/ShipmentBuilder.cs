using Logitude.Test.Base.Models;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Models.Builders
{
    public class ShipmentBuilder
    {
        private ShipmentPM _shipmentPM;

        public ShipmentBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _shipmentPM = new ShipmentPM();
        }

        public ShipmentBuilder Id(string id)
        {
            _shipmentPM.Id = id;
            return this;
        }

        public ShipmentBuilder Tenant(int tenant)
        {
            _shipmentPM.Tenant = tenant;
            return this;
        }

        public ShipmentBuilder ShipmentNumber(string shipmentNumber)
        {
            _shipmentPM.ShipmentNumber = shipmentNumber;
            return this;
        }

        public ShipmentBuilder CustomerId(string customerId)
        {
            _shipmentPM.CustomerId = customerId;
            return this;
        }

        public ShipmentBuilder MasterShipmentDataId(string masterShipmentDataId)
        {
            _shipmentPM.MasterShipmentDataId = masterShipmentDataId;
            return this;
        }

        public ShipmentBuilder MasterShipmentNumber(string masterShipmentNumber)
        {
            _shipmentPM.MasterShipmentNumber = masterShipmentNumber;
            return this;
        }

        public ShipmentBuilder NewConcurrencyGUID(string newConcurrencyGUID)
        {
            _shipmentPM.NewConcurrencyGUID = newConcurrencyGUID;
            return this;
        }

        public ShipmentBuilder DirectionId(string directionId)
        {
            _shipmentPM.DirectionId = directionId;
            return this;
        }

        public ShipmentBuilder TransportModeId(string transportModeId)
        {
            _shipmentPM.TransportModeId = transportModeId;
            return this;
        }

        public ShipmentBuilder ShipmentLevelCode(string shipmentLevelCode)
        {
            _shipmentPM.ShipmentLevelCode = shipmentLevelCode;
            return this;
        }

        public ShipmentBuilder BranchId(string branchId)
        {
            _shipmentPM.BranchId = branchId;
            return this;
        }

        public ShipmentBuilder DepartmentId(string departmentId)
        {
            _shipmentPM.DepartmentId = departmentId;
            return this;
        }

        public ShipmentBuilder FreightPrepaidCollectId(string freightPrepaidCollectId)
        {
            _shipmentPM.FreightPrepaidCollectId = freightPrepaidCollectId;
            return this;
        }

        public ShipmentBuilder OtherPrepaidCollectId(string otherPrepaidCollectId)
        {
            _shipmentPM.OtherPrepaidCollectId = otherPrepaidCollectId;
            return this;
        }

        public ShipmentBuilder CreatedByUserId(string createdByUserId)
        {
            _shipmentPM.CreatedByUserId = createdByUserId;
            return this;
        }

        public ShipmentBuilder UpdatedByUserId(string updatedByUserId)
        {
            _shipmentPM.UpdatedByUserId = updatedByUserId;
            return this;
        }

        public ShipmentBuilder MainCarriageToPortId(string mainCarriageToPortId)
        {
            _shipmentPM.MainCarriageToPortId = mainCarriageToPortId;
            return this;
        }

        public ShipmentBuilder MainCarriageToPortIdByCode(string mainCarriageToPortCode)
        {
            _shipmentPM.MainCarriageToPortId = PortCodeMapping(mainCarriageToPortCode);//ShipmentMapping.Ports.ContainsKey(mainCarriageToPortCode) ? ShipmentMapping.Ports[mainCarriageToPortCode] : null;
            return this;
        }

        public ShipmentBuilder MainCarriageFromPortId(string mainCarriageFromPortId)
        {
            _shipmentPM.MainCarriageFromPortId = mainCarriageFromPortId;
            return this;
        }

        public ShipmentBuilder MainCarriageFromPortIdByCode(string mainCarriageFromPortCode)
        {
            _shipmentPM.MainCarriageFromPortId = PortCodeMapping(mainCarriageFromPortCode);//ShipmentMapping.Ports.ContainsKey(mainCarriageFromPortCode) ? ShipmentMapping.Ports[mainCarriageFromPortCode] : null; ;
            return this;
        }

        public ShipmentBuilder PackagesQuantity(int? packagesQuantity)
        {
            _shipmentPM.PackagesQuantity = packagesQuantity;
            return this;
        }

        public ShipmentBuilder ConcurrencyGUID(string concurrencyGUID)
        {
            _shipmentPM.ConcurrencyGUID = concurrencyGUID;
            return this;
        }

        public ShipmentBuilder ShipmentPackages(List<ShipmentPackagePM> shipmentPackages)
        {
            _shipmentPM.ShipmentPackages = new List<ShipmentPackagePM>();
            _shipmentPM.ShipmentPackages.AddRange(shipmentPackages);
            return this;
        }

        public ShipmentBuilder ShipmentPayables(List<ShipmentPayablesPM> shipmentPayables)
        {
            _shipmentPM.ShipmentPayables = new List<ShipmentPayablesPM>();
            _shipmentPM.ShipmentPayables.AddRange(shipmentPayables);
            return this;
        }

        public ShipmentPM Build()
        {
            ShipmentPM result = _shipmentPM;

            this.Reset();

            return result;
        }

        public ShipmentBuilder WithDefualtValues()
        {
            _shipmentPM = new ShipmentPM
            {
                Tenant = UserTenant.Tenant,
                NewConcurrencyGUID = Guid.NewGuid().ToString(),
                BranchId = UserTenant.BranchId,
                DepartmentId = UserTenant.DepartmentId,
                CreatedByUserId = UserTenant.LoginUserId,
                UpdatedByUserId = UserTenant.LoginUserId
            };
            return this;
        }

        public ShipmentBuilder FromDataTable(Table chargeTypeData)
        {
            _shipmentPM = chargeTypeData.CreateInstance<ShipmentPM>();
            return this;
        }

        private string PortCodeMapping(string portCode)
        {
            switch(portCode)
            {
                case "LHR":
                    return ShipmentData.PortLHRId;
                case "MIA":
                    return ShipmentData.PortMIAId;
                case "JFK":
                    return ShipmentData.PortJFKId;
                case "SOU":
                    return ShipmentData.PortSOUId;
                case "NYC":
                    return ShipmentData.PortNYCId;
                case "LON":
                    return ShipmentData.PortLONId;
                case "MAN":
                    return ShipmentData.PortMANId;
                default:
                    return null;
            }
        }
    }
}
