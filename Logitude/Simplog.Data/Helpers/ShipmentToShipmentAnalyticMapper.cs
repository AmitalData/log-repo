using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.Helpers
{
    public static class ShipmentToShipmentAnalyticMapper
    {
        public static ShipmentAnalytic Map(Shipment shipment)
        {
            ShipmentAnalytic shipmentAnalytic = new ShipmentAnalytic();
            shipmentAnalytic.Id = shipment.Id;
            shipmentAnalytic.Tenant = shipment.Tenant;
            shipmentAnalytic.CreateDateTime = shipment.CreateDateTime;
            shipmentAnalytic.ShipmentNumber = shipment.ShipmentNumber;
            shipmentAnalytic.BranchId = shipment.BranchId;
            shipmentAnalytic.IncotermId = shipment.IncotermId;
            shipmentAnalytic.SalesmanUserId = shipment.SalesmanUserId;
            shipmentAnalytic.AccountManagerUserId = shipment.AccountManagerUserId;
            shipmentAnalytic.ShipmentTypeId = shipment.ShipmentTypeId;
            shipmentAnalytic.TransportModeId = shipment.TransportModeId;
            shipmentAnalytic.DirectionId = shipment.DirectionId;
            shipmentAnalytic.ShipmentLevelCode = shipment.ShipmentLevelCode;
            shipmentAnalytic.ShipperId = shipment.ShipperId;
            shipmentAnalytic.ConsigneeId = shipment.ConsigneeId;
            shipmentAnalytic.AgentId = shipment.AgentId;
            shipmentAnalytic.CustomerId = shipment.CustomerId;
            shipmentAnalytic.IsOperationalClosed = shipment.IsOperationalClosed;
            shipmentAnalytic.IsAccountingClosed = shipment.IsAccountingClosed;
            shipmentAnalytic.GrossWeightInKG = shipment.GrossWeightInKG;
            shipmentAnalytic.ChargeableWeightInKG = shipment.ChargeableWeightInKG;
            shipmentAnalytic.VolumeInCBM = shipment.VolumeInCBM;
            shipmentAnalytic.TEU = shipment.TEU;
            shipmentAnalytic.StatusId = shipment.StatusId;
            shipmentAnalytic.IsCancelled = shipment.IsCancelled;
            shipmentAnalytic.OpenReceivablesInLocalCurrency = shipment.OpenReceivablesInLocalCurrency;
            shipmentAnalytic.AccountedReceivablesInLocalCurrency = shipment.AccountedReceivablesInLocalCurrency;
            shipmentAnalytic.ProfitInLocalCurrency = shipment.ProfitInLocalCurrency;
            shipmentAnalytic.ProfitInProfitCurrency = shipment.ProfitInProfitCurrency;
            shipmentAnalytic.OpenReceivablesInProfitCurrency = shipment.OpenReceivablesInProfitCurrency;
            shipmentAnalytic.AccountedReceivablesInProfitCurrency = shipment.AccountedReceivablesInProfitCurrency;
            shipmentAnalytic.OpenPayablesInLocalCurrency = shipment.OpenPayablesInLocalCurrency;
            shipmentAnalytic.AccountedPayablesInLocalCurrency = shipment.AccountedPayablesInLocalCurrency;
            shipmentAnalytic.OpenPayablesInProfitCurrency = shipment.OpenPayablesInProfitCurrency;
            shipmentAnalytic.AccountedPayablesInProfitCurrency = shipment.AccountedPayablesInProfitCurrency;
            shipmentAnalytic.ARInvoiceIssued = shipment.ARInvoiceIssued;
            shipmentAnalytic.ShipmentSubTypeId = shipment.ShipmentSubTypeId;
            shipmentAnalytic.NumberOfContainers = shipment.NumberOfContainers;
            shipmentAnalytic.NumberOfPackages = shipment.NumberOfPackages;
            shipmentAnalytic.OperationalDate = shipment.OperationalDate;

            MapPorts(shipment, shipmentAnalytic);
            MapMasterDataFields(shipment.ShipmentMasterData, shipmentAnalytic);
            MapCountryFields(shipment, shipmentAnalytic);
            return shipmentAnalytic;
        }

        private static void MapPorts(Shipment shipment, ShipmentAnalytic shipmentAnalytic)
        {
            if(shipment.ShipmentMasterData == null)
            {
                shipmentAnalytic.MainCarriageFromPortId = shipment.FromPortId;
                shipmentAnalytic.MainCarriageToPortId = shipment.ToPortId;
            }
            else
            {
                shipmentAnalytic.MainCarriageFromPortId = shipment.ShipmentMasterData.MainCarriageFromPortId;
                shipmentAnalytic.MainCarriageToPortId = shipment.ShipmentMasterData.MainCarriageToPortId;
            }
        }

        private static void MapCountryFields(Shipment shipment, ShipmentAnalytic shipmentAnalytic)
        {
            bool isInlandDomesticShipment = (shipment.DirectionId == "D" && shipment.TransportModeId == "I");
            var commonDataContext = CommonDataContext.GetContext(shipment.Tenant);
            if (isInlandDomesticShipment)
            {
                AddressRepository addressRepository = new AddressRepository(commonDataContext);
                shipmentAnalytic.FromCountryId = shipment.ShipmentMasterData.MainCarriageFromAddressId == null ? null : addressRepository.GetSingleAddress(shipment.ShipmentMasterData.MainCarriageFromAddressId, shipment.Tenant)?.CountryId;
                shipmentAnalytic.ToCountryId = shipment.ShipmentMasterData.MainCarriageToAddressId == null ? null : addressRepository.GetSingleAddress(shipment.ShipmentMasterData.MainCarriageToAddressId, shipment.Tenant)?.CountryId;
                return;
            }
            PortRepository portsRep = new PortRepository(commonDataContext);
            if (shipment.ShipmentMasterData == null)
            {
                shipmentAnalytic.FromCountryId = shipment.FromPortId == null ? null : portsRep.GetSinglePort(shipment.FromPortId, shipment.Tenant)?.CountryId;
                shipmentAnalytic.ToCountryId = shipment.ToPortId == null ? null : portsRep.GetSinglePort(shipment.ToPortId, shipment.Tenant)?.CountryId;
                return;
            }
            shipmentAnalytic.FromCountryId = shipment.ShipmentMasterData.MainCarriageFromPortId == null ? null : portsRep.GetSinglePort(shipment.ShipmentMasterData.MainCarriageFromPortId, shipment.Tenant)?.CountryId;
            shipmentAnalytic.ToCountryId = shipment.ShipmentMasterData.MainCarriageToPortId == null ? null : portsRep.GetSinglePort(shipment.ShipmentMasterData.MainCarriageToPortId, shipment.Tenant)?.CountryId;
        }

        private static void MapMasterDataFields(ShipmentMasterData shipmentMasterData, ShipmentAnalytic shipmentAnalytic)
        {
            if (shipmentMasterData == null) return;
            shipmentAnalytic.MainCarriageCarrierId = shipmentMasterData.MainCarriageCarrierId;
        }
    }
}
