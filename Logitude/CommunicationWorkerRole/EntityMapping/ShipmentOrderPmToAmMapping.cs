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
        private readonly PackageTypeRepository packageTypeRepository;
        private readonly int tenant;

        public ShipmentOrderPmToAmMapping(int tenant)
        {
            ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
            cardsReporistory = new CardRepository(commoncontext);
            incotermRepository = new IncotermRepository(commoncontext);
            packageTypeRepository = new PackageTypeRepository(commoncontext);
            this.tenant = tenant;
        }

        public ShipmentOrderAM Map(int tenant, ShipmentOrderPM shipmentOrder)
        {
            Card agent = cardsReporistory.GetSingleCard(shipmentOrder.AgentId, tenant);
            Card shipper = cardsReporistory.GetSingleCard(shipmentOrder.ShipperId, tenant);

            ShipmentOrderAM shipmentOrderAM = new ShipmentOrderAM();
            shipmentOrderAM.Id = shipmentOrder.Id;
            shipmentOrderAM.CustomerShipmentNumber = shipmentOrder.CustomerShipmentNumber;
            shipmentOrderAM.CustomerTenantNumber = shipmentOrder.CustomerTenantNumber.Value;
            shipmentOrderAM.Volume = shipmentOrder.Volume;
            shipmentOrderAM.TransportModeId = shipmentOrder.TransportModeId;
            shipmentOrderAM.OrderNumber = shipmentOrder.OrderNumber;
            shipmentOrderAM.CustomerReferences = shipmentOrder.CustomerReferences;
            shipmentOrderAM.AgentName = agent?.EnglishName;
            shipmentOrderAM.MainCarriageATA = shipmentOrder.ATA;
            shipmentOrderAM.MainCarriageATD = shipmentOrder.ATD;
            shipmentOrderAM.MainCarriageETA = shipmentOrder.ETA;
            shipmentOrderAM.MainCarriageETD = shipmentOrder.ETD;
            shipmentOrderAM.Quantity = shipmentOrder.Quantity;
            shipmentOrderAM.Weight = shipmentOrder.GrossWeight;
            shipmentOrderAM.PackageTypeCode = GetPackageTypeCode(shipmentOrder.PackageTypeId);

            shipmentOrderAM.Shipper = GetShipper(shipper);
            shipmentOrderAM.ShipperName = shipper?.EnglishName;
            shipmentOrderAM.Incoterm = GetIncoterm(shipmentOrder);

            return shipmentOrderAM;
        }

        private string GetPackageTypeCode(string packageTypeId)
        {
            if (packageTypeId == null) return null;
            return packageTypeRepository.GetSinglePackageType(packageTypeId, tenant)?.Code;
        }

        private CodeProperties GetShipper(Card card)
        {
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
