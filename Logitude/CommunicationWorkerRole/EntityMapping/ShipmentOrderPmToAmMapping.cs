using Logitude.ShipmentOrderModule.Def.EntityAMs;
using Logitude.ShipmentOrderModule.Def.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;

namespace CommunicationWorkerRole.EntityMapping
{
    public class ShipmentOrderPmToAmMapping
    {
        private readonly CardRepository cardsReporistory;
        private readonly IncotermRepository incotermRepository;
        private readonly int tenant;

        public ShipmentOrderPmToAmMapping(int tenant)
        {
            ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
            cardsReporistory = new CardRepository(commoncontext);
            incotermRepository = new IncotermRepository(commoncontext);
            this.tenant = tenant;
        }

        public ShipmentOrderAM Map(int tenant, ShipmentOrderPM shipmentOrder)
        {
            Card agent = cardsReporistory.GetSingleCard(shipmentOrder.AgentId, tenant);

            ShipmentOrderAM shipmentOrderAM = new ShipmentOrderAM();
            shipmentOrderAM.Id = shipmentOrder.Id;
            shipmentOrderAM.CustomerShipmentNumber = shipmentOrder.CustomerShipmentNumber;
            shipmentOrderAM.CustomerTenantNumber = shipmentOrder.CustomerTenantNumber.Value;
            shipmentOrderAM.Volume = shipmentOrder.Volume;
            shipmentOrderAM.TransportModeId = shipmentOrder.TransportModeId;
            shipmentOrderAM.OrderNumber = shipmentOrder.OrderNumber;
            shipmentOrderAM.AgentName = agent?.EnglishName;
            shipmentOrderAM.MainCarriageATA = shipmentOrder.ATA;
            shipmentOrderAM.MainCarriageATD = shipmentOrder.ATD;
            shipmentOrderAM.MainCarriageETA = shipmentOrder.ETA;
            shipmentOrderAM.MainCarriageETD = shipmentOrder.ETD;
            shipmentOrderAM.Quantity = shipmentOrder.Quantity;
            shipmentOrderAM.Weight = shipmentOrder.GrossWeight;

            shipmentOrderAM.Shipper = GetShipper(shipmentOrder);
            shipmentOrderAM.ShipperName = shipmentOrder.ShipperName;
            shipmentOrderAM.Incoterm = GetIncoterm(shipmentOrder);

            return shipmentOrderAM;
        }

        private CodeProperties GetShipper(ShipmentOrderPM shipmentOrder)
        {
            Card card = cardsReporistory.GetSingleCard(shipmentOrder.ShipperId, tenant);
            if (card == null) return null;
            return new CodeProperties()
            {
                Code = card.Code
            };
        }

        private CodeProperties GetIncoterm(ShipmentOrderPM shipmentOrder)
        {
            Incoterm incoterm = incotermRepository.GetSingleIncoterm(shipmentOrder.IncotermId, tenant);
            if (incoterm == null) return null;
            return new CodeProperties()
            {
                Code = incoterm.Code
            };
        }
    }
}
