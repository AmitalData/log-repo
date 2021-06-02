using Logitude.Test.Base.Models.LocationsPreparation;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.CargoTrackingTests.Models.Builders
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
            _shipmentPM.MainCarriageToPortId = PortCodeMapping(mainCarriageToPortCode);
            return this;
        }

        public ShipmentBuilder MainCarriageFromPortId(string mainCarriageFromPortId)
        {
            _shipmentPM.MainCarriageFromPortId = mainCarriageFromPortId;
            return this;
        }

        public ShipmentBuilder MainCarriageFromPortIdByCode(string mainCarriageFromPortCode)
        {
            _shipmentPM.MainCarriageFromPortId = PortCodeMapping(mainCarriageFromPortCode);
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

        public ShipmentBuilder AgentAddressCountryCode(string agentAddressCountryCode)
        {
            _shipmentPM.AgentAddressCountryCode = agentAddressCountryCode;
            return this;
        }

        public ShipmentBuilder AgentName(string agentName)
        {
            _shipmentPM.AgentName = agentName;
            return this;
        }

        public ShipmentBuilder AgentId(string PartnerType)
        {
            _shipmentPM.AgentId = PartnerType == "AG" ? PartnersData.AgentId : null;
            return this;
        }

        public ShipmentBuilder ShipmentPackages(List<PackagePM> shipmentPackages)
        {
            _shipmentPM.ShipmentPackages.AddRange(shipmentPackages);
            return this;
        }

        public ShipmentBuilder ShipmentPackages(PackagePM shipmentPackage)
        {
            _shipmentPM.ShipmentPackages.Add(shipmentPackage);
            return this;
        }

        public ShipmentBuilder ShipmentPayables(List<PayablesPM> shipmentPayables)
        {
            _shipmentPM.ShipmentPayables.AddRange(shipmentPayables);
            return this;
        }

        public ShipmentBuilder ShipmentPayables(PayablesPM shipmentPayable)
        {
            _shipmentPM.ShipmentPayables.Add(shipmentPayable);
            return this;
        }

        public ShipmentBuilder ShipmentReceivable(List<ShipmentReceivablePM> shipmentReceivable)
        {
            _shipmentPM.ShipmentReceivable.AddRange(shipmentReceivable);
            return this;
        }

        public ShipmentBuilder ShipmentReceivable(ShipmentReceivablePM shipmentReceivable)
        {
            if (_shipmentPM.ShipmentReceivable == null)
            {
                _shipmentPM.ShipmentReceivable = new List<ShipmentReceivablePM>();
            }
            _shipmentPM.ShipmentReceivable.Add(shipmentReceivable);
            return this;
        }
        public ShipmentBuilder ShipmentReceivableStatusCode(string ShipmentReceivableStatusCode)
        {
            _shipmentPM.ShipmentReceivableStatusCode = ShipmentReceivableStatusCode;
            return this;
        }
        public ShipmentBuilder ShipmentReceivableStatusName(string ShipmentReceivableStatusName)
        {
            _shipmentPM.ShipmentReceivableStatusName = ShipmentReceivableStatusName;
            return this;
        }
        public ShipmentBuilder GrossWeight(double? grossWeight)
        {
            _shipmentPM.GrossWeight = grossWeight;
            return this;
        }

        public ShipmentPM Build()
        {
            ShipmentPM result = _shipmentPM;

            this.Reset();

            return result;
        }

        public ShipmentBuilder WithModel(ShipmentPM shipmentPM)
        {
            _shipmentPM = shipmentPM;
            return this;
        }

        public ShipmentBuilder WithDefualtValues()
        {
            _shipmentPM = new ShipmentPM
            {
                Tenant = UserTenant.Tenant,
                NewConcurrencyGUID = Guid.NewGuid().ToString(),
                BranchId = UserTenant.BranchId,
                DepartmentId = UserTenant.DepartmentId,
                CustomerId = PartnersData.CustomerId,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId
            };
            return this;
        }

        public ShipmentBuilder FromDataTable(Table dataTable)
        {
            _shipmentPM = dataTable.CreateInstance<ShipmentPM>();
            return this;
        }

        private string PortCodeMapping(string portCode)
        {
            switch (portCode)
            {
                case "LHR":
                    return LocationsData.PortLHRId;
                case "MIA":
                    return LocationsData.PortMIADomesticId;
                case "JFK":
                    return LocationsData.PortAirJFKId;
                case "SOU":
                    return LocationsData.PortOceanSOUId;
                case "NYC":
                    return LocationsData.PortInlandNYCId;
                case "LON":
                    return LocationsData.PortLONId;
                case "MAN":
                    return LocationsData.PortMANId;
                default:
                    return null;
            }
        }
    }
}
