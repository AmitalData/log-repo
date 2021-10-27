using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CargoTracking.Def.EntityPMs;
using Logitude.ShipmentOrderModule.BL.EntityQueryServices;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.CargoTracking.BL.EntityQueryServices
{
    public class CargoTrackingShipmenPartnersCardsBuilder
    {
        const string ShipmentOrderEntityType = "O";
        private const string AddressSeparator = "<br>";
        readonly List<PartnerCardMetaData> partnerCardsMetaDatas = new List<PartnerCardMetaData>
            {
                new PartnerCardMetaData("Releasing Agent", "ReleasingAgentId", "ReleasingAgentName"),
                new PartnerCardMetaData("Carrier", "MainCarriageCarrierId", "MainCarriageCarrierName"),
                new PartnerCardMetaData("Carrier", "CarrierId", "CarrierName", true),
                new PartnerCardMetaData("Warehouse", "WarehouseLegWarehouseId", "WarehouseLegTerminalName"),
                new PartnerCardMetaData("Trucker", "CarrierId", "CarrierName"),
                new PartnerCardMetaData("Consolidator", "ConsolidatorId", "ConsolidatorName"),
                new PartnerCardMetaData("Freelancer", "FreelancerId", "FreelancerName"),
                new PartnerCardMetaData("Coloader", "ColoaderId", "ColoaderName"),
                new PartnerCardMetaData("Custom Clearance Point", "CustomClearancePointId", "CustomClearancePointName"),
                new PartnerCardMetaData("Consignee Not Importer", "ConsigneeNotImporterId", "ConsigneeNotImporterName"),
                new PartnerCardMetaData("Shipper Not Exporter", "ShipperNotExporterId", "ShipperNotExporterName"),
                new PartnerCardMetaData("Notify 2", "Notify2Id", "Notify2Name"),
                new PartnerCardMetaData("Notify 1", "Notify1Id", "Notify1Name"),
                new PartnerCardMetaData("Custom Agent Import", "CustomAgentImportId", "CustomAgentImportName"),
                new PartnerCardMetaData("Custom Agent Export", "CustomAgentExportId", "CustomAgentExportName"),
                new PartnerCardMetaData("Issuing Carrier's Agent", "IssuingCarrierAgentId", "IssuingCarrierAgentName"),
                new PartnerCardMetaData("Agent", "AgentName", "AgentId"),
                new PartnerCardMetaData("Agent", "AgentName", "AgentId", true),
                new PartnerCardMetaData("Customer", "CustomerId", "CustomerName"),
                new PartnerCardMetaData("Freight Forwarder", "FreightForwarderId", "FreightForwarderName"),
                new PartnerCardMetaData("consignee", "ConsigneeId", "ConsigneeName"),
                new PartnerCardMetaData("consignee", "ConsigneeId", "ConsigneeName", true),
                new PartnerCardMetaData("shipper", "ShipperId", "ShipperName"),
                new PartnerCardMetaData("shipper", "ShipperId", "ShipperName", true)
            };
        List<AddressList> partnersAddresses;
        List<PartnerCard> partnerCards = new List<PartnerCard>();
        ShipmentOrderPM shipmentOrderPM;
        ShipmentPM shipmentPM;
        CargoTrackingShipmentPM cargoShipmentPM;

        public CargoTrackingShipmenPartnersCardsBuilder(ShipmentOrderPM shipmentOrderPM, ShipmentPM shipmentPM, CargoTrackingShipmentPM cargoShipmentPM)
        {
            this.shipmentOrderPM = shipmentOrderPM;
            this.shipmentPM = shipmentPM;
            this.cargoShipmentPM = cargoShipmentPM;
        }

        public List<PartnerCard> BuildPartnerCards()
        {
            GetPartnersAddresses();
            foreach (var partnerCardMetaData in partnerCardsMetaDatas)
            {
                CreatePartnerCard(partnerCardMetaData);
            }

            return partnerCards;
        }

        private void GetPartnersAddresses()
        {
            var partnersIds = GetShipmentPartnersIds(shipmentPM, cargoShipmentPM, shipmentOrderPM);
            AddressQuery addressQuery = new AddressQuery(cargoShipmentPM.Tenant);
            partnersAddresses = addressQuery.GetAddressesByCardIds(partnersIds, cargoShipmentPM.Tenant);
        }
        private List<string> GetShipmentPartnersIds(ShipmentPM shipmentPM, CargoTrackingShipmentPM cargoShipmentPM, ShipmentOrderPM shipmentOrderPM)
        {
            if (cargoShipmentPM.EntityType == ShipmentOrderEntityType)
            {
                return new List<string>() {
                    shipmentOrderPM.ShipperId,
                    shipmentOrderPM.ConsigneeId,
                    shipmentOrderPM.AgentId,
                    shipmentOrderPM.CarrierId
                };
            }
            else
            {

                List<string> partnersIds = new List<string>() {
                    shipmentPM.ShipperId,
                    shipmentPM.ConsigneeId,
                    shipmentPM.FreightForwarderId,
                    shipmentPM.CustomerId,
                    shipmentPM.AgentId,
                    shipmentPM.IssuingCarrierAgentId,
                    shipmentPM.CustomAgentExportId,
                    shipmentPM.CustomAgentImportId,
                    shipmentPM.Notify1Id,
                    shipmentPM.Notify2Id,
                    shipmentPM.ShipperNotExporterId,
                    shipmentPM.ConsigneeNotImporterId,
                    shipmentPM.CustomClearancePointId,
                    shipmentPM.ColoaderId,
                    shipmentPM.FreelancerId,
                    shipmentPM.ConsolidatorId,
                    shipmentPM.ReleasingAgentId,
                    shipmentPM.MainCarriageCarrierId,
                    shipmentPM.WarehouseLegWarehouseId,
                    cargoShipmentPM.ShipperId
                };

                partnersIds.AddRange(shipmentPM.ShipmentDeliveries.Select(d => d.CarrierId).ToList());
                partnersIds.AddRange(shipmentPM.ShipmentPickUps.Select(d => d.CarrierId).ToList());
                return partnersIds;
            }

        }

        private void CreatePartnerCard(PartnerCardMetaData partnerCardMetaData)
        {
            if (partnerCardMetaData.IsFromShipmentOrder && shipmentOrderPM != null)
                CreateShipmetOrderPartnerCard(partnerCardMetaData);
            else
                CreateShipmentPartnerCard(partnerCardMetaData);
        }

        private void CreateShipmentPartnerCard(PartnerCardMetaData partnerCardMetaData)
        {
            var id = GetProperty(shipmentPM, partnerCardMetaData.IdFieldName);
            if (id != null)
                partnerCards.Add(new PartnerCard()
                {
                    Type = partnerCardMetaData.TypeName,
                    Name = GetProperty(shipmentPM, partnerCardMetaData.NameFieldName),
                    Address = GetPartnerAddress(id),
                    PhoneNumber = GetPartnerPhoneNumberFromAddress(id),
                    FaxNumber = GetPartnerFaxNumberFromAddress(id),
                });
        }

        private void CreateShipmetOrderPartnerCard(PartnerCardMetaData partnerCardMetaData)
        {
            var id = GetProperty(shipmentOrderPM, partnerCardMetaData.IdFieldName);
            if (id != null)
                partnerCards.Add(new PartnerCard()
                {
                    Type = partnerCardMetaData.TypeName,
                    Name = GetProperty(shipmentOrderPM, partnerCardMetaData.NameFieldName),
                    Address = GetPartnerAddress(id),
                    PhoneNumber = GetPartnerPhoneNumberFromAddress(id),
                    FaxNumber = GetPartnerFaxNumberFromAddress(id),
                });
        }

        public string GetProperty(object target, string name)
        {
            return (string)target.GetType().GetProperty(name)?.GetValue(target, null);
        }


        private string GetPartnerAddress(string cardId)
        {
            var address = partnersAddresses.FirstOrDefault(a => a.CardId == cardId);
            if (address != null)
            {
                var addressLines = new List<string>() {
                    address.Address1,
                    address.Address2,
                    address.ZipCode,
                    address.City + ',' + address.CountryName
                    };
                return string.Join(AddressSeparator, addressLines);
            }
            return null;
        }
        private string GetPartnerFaxNumberFromAddress(string cardId)
        {
            var address = this.partnersAddresses.FirstOrDefault(a => a.CardId == cardId);
            if (address != null)
                return address.FaxNumber;
            return null;
        }
        private string GetPartnerPhoneNumberFromAddress(string cardId)
        {
            var address = this.partnersAddresses.FirstOrDefault(a => a.CardId == cardId);
            if (address != null)
                return address.PhoneNumber;
            return null;
        }
    }

    public class PartnerCardMetaData
    {
        public PartnerCardMetaData(string typeName, string idFieldName, string nameFieldName, bool isFromShipmentOrder = false)
        {
            TypeName = typeName;
            IdFieldName = idFieldName;
            NameFieldName = nameFieldName;
            IsFromShipmentOrder = isFromShipmentOrder;
        }
        public string TypeName { get; set; }
        public string IdFieldName { get; set; }
        public string NameFieldName { get; set; }
        public bool IsFromShipmentOrder { get; set; }
    }
   
}