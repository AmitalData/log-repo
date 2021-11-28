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
        private ShipmentPM shipmentPM;
        private HybridPartnerRepository hybridPartnerRepository;
        private CardRepository cardsReporistory;
        public ExporterShipmentAMMappingService(ShipmentPM shipment)
        {
            this.shipmentPM = shipment;
            ICommonDataContext commoncontext = CommonDataContext.GetContext(shipment.Tenant);
            hybridPartnerRepository = new HybridPartnerRepository(commoncontext);
            cardsReporistory = new CardRepository(commoncontext);
        }
        public NewAExporterShipmentAM GetMappedExportShipmentAM(int tenant)
        {
            HybridPartnerPM Partner = GetHybridPartner();
            NewAExporterShipmentAM newAExporterShipmentAM = GetNewExportShipmentAM(tenant, Partner);
            MapExportShipmentPackages(shipmentPM, newAExporterShipmentAM);
            MapShipmentShipper(newAExporterShipmentAM);
            MapShipmentCustomer(newAExporterShipmentAM);
            MapShipmentPorts(shipmentPM, newAExporterShipmentAM);
            MapPartners(newAExporterShipmentAM);

            return newAExporterShipmentAM;
        }

        private void MapPartners(NewAExporterShipmentAM newAExporterShipmentAM)
        {
            newAExporterShipmentAM.Agent = GetCard(shipmentPM.AgentId);
            newAExporterShipmentAM.Consignee = GetCard(shipmentPM.ConsigneeId);
        }

        private CodeProperties GetCard(string cardId)
        {
            Card card = cardsReporistory.GetSingleCard(cardId, shipmentPM.Tenant);
            return new CodeProperties()
            {
                Code = card != null ? card.Code : "",
            };

        }

        private HybridPartnerPM GetHybridPartner()
        {
            HybridPartnerQuery HybridPartnerQuerey = new HybridPartnerQuery(hybridPartnerRepository);
            HybridPartnerPM Partner = HybridPartnerQuerey.GetSinglePM(shipmentPM.ForwarderPartnerId);
            return Partner;
        }
        private NewAExporterShipmentAM GetNewExportShipmentAM(int tenant, HybridPartnerPM Partner)
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
                DangerousGoods = shipmentPM.IsDangerous,
                ReqFlightDate = shipmentPM.RequestedFlightDate,
                Quantity = shipmentPM.BookingNumberOfPackages,
                Weight = shipmentPM.OrderGrossWeight,
                Volume = shipmentPM.BookingVolume,
                Incoterm = shipmentPM.IncotermCode,
                Notes = shipmentPM.Notes,
            };
        }
        private static void MapExportShipmentPackages(ShipmentPM ForwarderShipment, NewAExporterShipmentAM newAExporterShipmentAM)
        {
            newAExporterShipmentAM.ShipmentPackages = new List<Packages>();
            foreach (var item in ForwarderShipment.ShipmentOrderPackages)
            {
                Packages Package = new Packages();
                Package.Quantity = item.Quantity;
                Package.GrossWeight = item.GrossWeight;
                Package.Length = item.Length;
                Package.Width = item.Width;
                Package.Height = item.Height;
                newAExporterShipmentAM.ShipmentPackages.Add(Package);
            }
        }
        private static void MapShipmentPorts(ShipmentPM ForwarderShipment, NewAExporterShipmentAM newAExporterShipmentAM)
        {
            newAExporterShipmentAM.FromPort = new CodeProperties()
            {
                Code = ForwarderShipment.FromPort,
                CountryCode = ForwarderShipment.FromCountryCode
            };
            newAExporterShipmentAM.ToPort = new CodeProperties()
            {
                Code = ForwarderShipment.ToPort,
                CountryCode = ForwarderShipment.ToCountryCode
            };
        }
        private void MapShipmentCustomer(NewAExporterShipmentAM newAExporterShipmentAM)
        {
            Card Customer = cardsReporistory.GetSingleCard(shipmentPM.CustomerId, shipmentPM.Tenant);
            string CustomerCode = "";
            if (Customer != null)
            {
                CustomerCode = Customer.Code;
            }
            newAExporterShipmentAM.Customer = new CodeProperties()
            {
                Code = CustomerCode
            };
        }
        private void MapShipmentShipper(NewAExporterShipmentAM newAExporterShipmentAM)
        {
            Card Shipper = cardsReporistory.GetSingleCard(shipmentPM.ShipperId, shipmentPM.Tenant);
            string ShipperCode = "";

            if (Shipper != null)
            {
                ShipperCode = Shipper.Code;
            }

            newAExporterShipmentAM.Shipper = new CodeProperties()
            {
                Code = ShipperCode
            };
        }
    }
}
