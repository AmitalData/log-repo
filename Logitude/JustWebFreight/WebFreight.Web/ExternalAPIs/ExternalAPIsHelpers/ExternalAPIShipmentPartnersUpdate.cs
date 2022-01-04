using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebFreight.Web.ExternalAPIs.ExternalAPIsHelpers
{
    public class ExternalAPIShipmentPartnersUpdate
    {
        private IShipmentsContext shipmentsContext;
        private Shipment shipment;
        private ShipmentPM shipmentPM;
        private Dictionary<string, string> patrnerProperties;
        private Type shipmentType;
        private Type shipmentPMType;
        private AddressRepository addressRepository;
        public ExternalAPIShipmentPartnersUpdate(IShipmentsContext shipmentsContext, ShipmentPM shipmentPM)
        {
            this.shipmentsContext = shipmentsContext;
            this.shipmentPM = shipmentPM;
            this.shipmentType = typeof(Shipment);
            this.shipmentPMType = typeof(ShipmentPM);
            this.addressRepository = new AddressRepository(shipmentPM.Tenant);
            this.GetShipmentPoco();
            this.SetPatrnerPropertiesNames();
        }


        public ShipmentPM UpdatePartners()
        {
            if (shipment == null)
                return shipmentPM;

            foreach (string partnerIdProperty in patrnerProperties.Values)
            {
                if (IsPartnerValueChanged(partnerIdProperty))
                {
                    UpdatePartnerAddressAndContact(partnerIdProperty);
                }
            }

            return shipmentPM;
        }

        private void GetShipmentPoco()
        {
            this.shipment = shipmentsContext.Shipments.Where(d => d.Id == shipmentPM.Id && d.Tenant == shipmentPM.Tenant).FirstOrDefault();
        }

        private void SetPatrnerPropertiesNames()
        {
            patrnerProperties = new Dictionary<string, string>();
            patrnerProperties.Add("Shipper", "ShipperId");
            patrnerProperties.Add("Consignee", "ConsigneeId");
            patrnerProperties.Add("ShipperNotExporter", "ShipperNotExporterId");
            patrnerProperties.Add("Agent", "AgentId");
            patrnerProperties.Add("CustomAgentImport", "CustomAgentImportId");
            patrnerProperties.Add("ReleasingAgent", "ReleasingAgentId");
            patrnerProperties.Add("FreightForwarder", "FreightForwarderId");
            patrnerProperties.Add("CustomAgentExport", "CustomAgentExportId");
            patrnerProperties.Add("ConsigneeNotImporter", "ConsigneeNotImporterId");
            patrnerProperties.Add("Notify1", "Notify1Id");

        }

        private bool IsPartnerValueChanged(string partnerIdProperty)
        {
            PropertyInfo partnerPocoProperty = this.shipmentType.GetProperty(partnerIdProperty);
            PropertyInfo partnerPMProperty = this.shipmentPMType.GetProperty(partnerIdProperty);

            var partnerPocoPropertyValue = partnerPocoProperty.GetValue(shipment);
            var partnerPMPropertyValue = partnerPMProperty.GetValue(shipmentPM);

            if (partnerPMPropertyValue == null && partnerPocoPropertyValue == null)
                return false;

            if (partnerPMPropertyValue.Equals(partnerPocoPropertyValue))
                return false;

            return true;
        }

        private void UpdatePartnerAddressAndContact(string partnerIdProperty)
        {
            PropertyInfo partnerPMProperty = this.shipmentPMType.GetProperty(partnerIdProperty);
            var partnerPMPropertyValue = partnerPMProperty.GetValue(shipmentPM);
            if (partnerPMPropertyValue == null)
                return;

            Card card = CardRepository.GetSingleCard(partnerPMPropertyValue.ToString(), shipmentPM.Tenant, false);
            this.MapPartnerContact(card, partnerIdProperty);
            this.MapPartnerAddress(card, partnerIdProperty);
        }

        private void MapPartnerContact(Card card,string partnerIdProperty)
        {
            if (card == null)
            {
                return;
            }
            string partnerContactPMPropertyName = patrnerProperties.FirstOrDefault(x => x.Value == partnerIdProperty).Key + "ContactId";
            PropertyInfo partnerContactPMProperty = this.shipmentPMType.GetProperty(partnerContactPMPropertyName);
            if (partnerContactPMProperty == null)
                return;

            partnerContactPMProperty.SetValue(shipmentPM, card.PrimaryContactId);
        }

        private void MapPartnerAddress(Card card, string partnerIdProperty)
        {
            if (card == null)
                return;

            string partnerAddressPMPropertyName = patrnerProperties.FirstOrDefault(x => x.Value == partnerIdProperty).Key + "AddressId";
            PropertyInfo partnerAddressPMProperty = this.shipmentPMType.GetProperty(partnerAddressPMPropertyName);
            if (partnerAddressPMProperty == null)
                return;

            partnerAddressPMProperty.SetValue(shipmentPM, addressRepository.GetMainAddressId(card.Id, shipmentPM.Tenant));
        }
    }
}