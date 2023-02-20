using Logitude.Server.Tools.AnalyticTableServices;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System.Data.Entity;

namespace Logitude.BL.AnalyticTableServices
{
    public class ShipmentAnalyticTableService : AnalyticTableService<Shipment, ShipmentAnalytic>
    {
        public ShipmentAnalyticTableService(DbContext context) : base(context)
        {

        }

        protected override void CustomMap(Shipment entity, ShipmentAnalytic analyticTable)
        {
            MapPorts(entity, analyticTable);
            MapMasterDataFields(entity.ShipmentMasterData, analyticTable);
            MapCountryFields(entity, analyticTable);
        }

        private static void MapPorts(Shipment shipment, ShipmentAnalytic shipmentAnalytic)
        {
            if (shipment.ShipmentMasterData == null)
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
