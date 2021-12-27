using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityAMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;

namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class ExporterShipmentAMMappingService
    {
        private readonly ShipmentPM shipmentPM;
        private CardRepository cardsReporistory;
        private readonly int tenant;
        private HybridPartnerPM Partner;
        public ExporterShipmentAMMappingService(ShipmentPM shipmentPM)
        {
            this.shipmentPM = shipmentPM;
            tenant = shipmentPM.Tenant;
            Initialize();
        }

        private void Initialize()
        {
            ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
            cardsReporistory = new CardRepository(commoncontext);
            Partner = GetHybridPartner();
        }

        private HybridPartnerPM GetHybridPartner()
        {
            HybridPartnerQuery HybridPartnerQuerey = new HybridPartnerQuery(tenant);
            HybridPartnerPM Partner = HybridPartnerQuerey.GetSinglePM(shipmentPM.ForwarderPartnerId);
            return Partner;
        }

        public object GetMappedExportShipmentAM()
        {
            switch (shipmentPM.TransportModeId)
            {
                case "A": return GetNewAExportShipmentAM();
                case "O": return GetNewOExportShipmentAM();
                default: return null;
            }
        }

        private NewAExporterShipmentAM GetNewAExportShipmentAM()
        {
            return new NewAExporterShipmentAM()
            {
                Id = shipmentPM.Id,
                ExporterTenant = tenant,
                Tenant = (int)Partner.PartnerTenant,
                TransportModeId = shipmentPM.TransportModeId,
                DirectionId = shipmentPM.DirectionId,
                CustomerShipmentNumber = shipmentPM.ShipmentNumber,
                ShipmentTypeId = shipmentPM.ShipmentTypeId,
                ConsigneeName = shipmentPM.ConsigneeName,
                InvoiceReference = shipmentPM.PrivateLabelInvoiceNumber,
                CustomerReference = !string.IsNullOrEmpty(shipmentPM.CustomerReference3) ? shipmentPM.CustomerReference3 : shipmentPM.CustomerReference1,
                IncludePickup = shipmentPM.PrivateLabelIncludePickup,
                IncludeDelivery = shipmentPM.PrivateLabelIncludeDelivery,
                ReqFlightDate = shipmentPM.RequestedFlightDate,
                Quantity = shipmentPM.BookingNumberOfPackages,
                Weight = shipmentPM.OrderGrossWeight,
                Volume = shipmentPM.BookingVolume,
                Incoterm = shipmentPM.IncotermCode,
                Notes = shipmentPM.Notes,
                IsDangerouseOfGoods = shipmentPM.OrderIsDangerouseGoods,
                ShipmentPackages = GetShipmentPackages(),
                Shipper = GetCardById(shipmentPM.ShipperId),
                Customer = GetCardById(shipmentPM.CustomerId),
                FromPort = GetShipmentPort(shipmentPM.FromPort, shipmentPM.FromCountryCode),
                ToPort = GetShipmentPort(shipmentPM.ToPort, shipmentPM.ToCountryCode),
                Agent = GetCardById(shipmentPM.AgentId),
                Consignee = GetCardById(shipmentPM.ConsigneeId),
                AgentName = shipmentPM.PrivateLabelAgentName,

            };
        }
        
        private NewOExporterShipmentAM GetNewOExportShipmentAM()
        {
            return new NewOExporterShipmentAM()
            {
                Id = shipmentPM.Id,
                Tenant = (int)Partner.PartnerTenant,
                ExporterTenant = tenant,
                TransportModeId = shipmentPM.TransportModeId,
                DirectionId = shipmentPM.DirectionId,
                CustomerShipmentNumber = shipmentPM.ShipmentNumber,
                Shipper = GetCardById(shipmentPM.ShipperId),
                FromPort = GetShipmentPort(shipmentPM.FromPort, shipmentPM.FromCountryCode),
                ToPort = GetShipmentPort(shipmentPM.ToPort, shipmentPM.ToCountryCode),
                ShipmentTypeId = shipmentPM.ShipmentTypeId,
                Customer = GetCardById(shipmentPM.CustomerId),
                ConsigneeName = shipmentPM.ConsigneeName,
                ShippingAgent = shipmentPM.ShippingAgent,
                InvoiceReference = shipmentPM.PrivateLabelInvoiceNumber,
                CustomerReference = !string.IsNullOrEmpty(shipmentPM.CustomerReference3) ? shipmentPM.CustomerReference3 : shipmentPM.CustomerReference1,
                Notes = shipmentPM.Notes,
                IncludePickup = shipmentPM.PrivateLabelIncludePickup,
                IncludeDelivery = shipmentPM.PrivateLabelIncludeDelivery,
                DangerouseGoods = shipmentPM.OrderIsDangerouseGoods,
                Incoterm = shipmentPM.IncotermCode,
                ReqFlightDate = shipmentPM.RequestedFlightDate,
                Quantity = shipmentPM.BookingNumberOfPackages,
                Weight = shipmentPM.OrderGrossWeight,
                Volume = shipmentPM.BookingVolume,
                ShipmentPackages = GetShipmentPackages(),
            };
        }
        
        private List<Packages> GetShipmentPackages()
        {
            List<Packages> shipmentPMPackages = new List<Packages>();
            foreach (var shipmentPMPackage in shipmentPM.ShipmentOrderPackages)
            {
                shipmentPMPackages.Add(new Packages
                {
                    Quantity = shipmentPMPackage.Quantity,
                    GrossWeight = shipmentPMPackage.GrossWeight,
                    Length = shipmentPMPackage.Length,
                    Width = shipmentPMPackage.Width,
                    Height = shipmentPMPackage.Height,
                    Type = shipmentPM.TransportModeId == "O" ? shipmentPMPackage.PackageTypeCode : "",
                });
            }

            return shipmentPMPackages;
        }
       
        private CodeProperties GetCardById(string cardId)
        {
            Card card = cardsReporistory.GetSingleCard(cardId, tenant);
            
            return new CodeProperties()
            {
                Code = card != null ? card.Code : ""
            };
        }

        private CodeProperties GetShipmentPort(string portCode, string countryCode)
        {
            return new CodeProperties()
            {
                Code = portCode,
                CountryCode = countryCode
            };
        }
    }
}
