using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Behaviours;
using Logitude.BL.ShipmentsModel.Tools.TraceEvents;
using Logitude.WarehouseLib.Data;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviour
{
    public class UpdateCrossDockBehaviour : IShipmentBehaviour
    {
        private ShipmentPM shipmentPM;
        private IWarehouseContext warehouseContext;
        private WarehouseEntryRepository warehouseEntryRepository;
        private WarehouseReleaseRepository warehouseReleaseRepository;
        private IQueryable<WarehouseEntry> warehouseEntries;
        private IQueryable<WarehouseRelease> warehouseRelases;
        private int tenant;
        public UpdateCrossDockBehaviour(ShipmentPM shipmentPM)
        {
            this.shipmentPM = shipmentPM;
            this.tenant = shipmentPM.Tenant;

            warehouseContext = WarehouseContext.GetContext(tenant);
            warehouseEntryRepository = new WarehouseEntryRepository(warehouseContext);
            warehouseReleaseRepository = new WarehouseReleaseRepository(warehouseContext);
            warehouseEntries = warehouseEntryRepository.GetWarehouseEntriesByshipmentId(shipmentPM.Id, tenant);
            warehouseRelases = warehouseReleaseRepository.GetWarehouseReleasesByshipmentId(shipmentPM.Id, tenant);
        }

        public void Handle()
        {
            UpdateShipmentWarehouseLegData();
            UpdateWarehouseLegDates();
            UpdateDeliveryDepartureDates();
            UpdateCrossDockReleaseStatus();
            UpdateWareHouseEntryPartners();
            ConnectWarehouseReleaseToShipment();
        }
        public void Trace(ShipmentTracing shipmentTracing)
        {
            shipmentTracing.TraceTerminalData();
        }
        private void UpdateShipmentWarehouseLegData() 
        {
            if (shipmentPM.IsUpdateWarehouseLegData)
            {
                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                CardRepository cardRepository = new CardRepository(commonContext);
                Card entityPoco = cardRepository.GetSingleCardByIdAndTenant(shipmentPM.WarehouseLegWarehouseId, tenant,true);

                if (entityPoco != null)
                {
                    AddressRepository addressRepository = new AddressRepository(commonContext);              
                    Address mainAddress = addressRepository.GetSingleAddressByCardIdAndTypeId(entityPoco.Id, "M", tenant);
                    shipmentPM.WarehouseLegAddressId = mainAddress == null ? null : mainAddress.Id;
                    shipmentPM.WarehouseLegTerminalName = entityPoco.EnglishName;
                    shipmentPM.WarehouseLegTerminalCode = entityPoco.Warehouse != null ? entityPoco.Warehouse.FirmCode : null;

                    //TenantQuery tenantQuery = new TenantQuery(tenant);
                    //string countryCode = tenantQuery.GetTenantCountryCodeOnly(tenant);
                    //bool usTenant = countryCode == null ? false : countryCode.ToUpper() == "US" ? true : false;
                    //if(usTenant) shipmentPM.WarehouseLegTerminalCode = entityPoco.Warehouse != null ? entityPoco.Warehouse.FirmCode : null;
                }
            }
        }
        private void UpdateWarehouseLegDates()
        {
            UpdateActualExpectedWarehouseEntriesDates();
            UpdateActualExpectedWarehouseReleasesDates();
            UpdateActualExpectedShipmentWarehouseLegDates();
        }
        public void Save()
        {
            warehouseContext.SaveChanges();
        }
        private void UpdateActualExpectedWarehouseEntriesDates()
        {
            IQueryable<WarehouseEntry> activeWarehouseEntries = warehouseEntries.Where(e => e.StatusCode != "CAEA");
            if (activeWarehouseEntries.Count() != 0)
            {
                WarehouseEntry leastWarehouseEntry = activeWarehouseEntries.Where(e => e.ActualEntryDate != null).OrderBy(e => e.ActualEntryDate).FirstOrDefault();
                if (leastWarehouseEntry != null)
                {
                    shipmentPM.WarehouseLegActualEntryDate = leastWarehouseEntry.ActualEntryDate;
                    shipmentPM.WarehouseLegExpectedEntryDate = leastWarehouseEntry.ExpectedEntryDate;
                }
            }
            else if (warehouseEntries.Count() != 0)
            {
                shipmentPM.WarehouseLegActualEntryDate = null;
                shipmentPM.WarehouseLegExpectedEntryDate = null;
            }
        }
        private void UpdateActualExpectedWarehouseReleasesDates()
        {
            IQueryable<WarehouseRelease> activeWarehouseRelases = warehouseRelases.Where(e => e.StatusCode != "CARE");
            if (activeWarehouseRelases.Count() == 1)
            {
                WarehouseRelease warehouseRelease = activeWarehouseRelases.FirstOrDefault();
                if (shipmentPM.WarehouseLegActualReleaseDate != null && warehouseRelease.ActualReleaseDate == null)
                {
                    warehouseRelease.ActualReleaseDate = shipmentPM.WarehouseLegActualReleaseDate;
                }
                if (shipmentPM.WarehouseLegExpectedReleaseDate != null && warehouseRelease.ExpectedReleaseDate == null)
                {
                    warehouseRelease.ExpectedReleaseDate = shipmentPM.WarehouseLegExpectedReleaseDate;
                }
                warehouseReleaseRepository.Update(warehouseRelease);
            }
        }
        private void UpdateActualExpectedShipmentWarehouseLegDates()
        {
            IQueryable<WarehouseRelease> activeWarehouseRelases = warehouseRelases.Where(e => e.StatusCode != "CARE");
            if (activeWarehouseRelases.Count() != 0)
            {
                WarehouseRelease greatestWarehouseRelease = activeWarehouseRelases.Where(r => r.ActualReleaseDate != null).OrderByDescending(r => r.ActualReleaseDate).FirstOrDefault();
                if (greatestWarehouseRelease != null)
                {
                    shipmentPM.WarehouseLegActualReleaseDate = greatestWarehouseRelease.ActualReleaseDate;
                    shipmentPM.WarehouseLegExpectedReleaseDate = greatestWarehouseRelease.ExpectedReleaseDate;
                }
            }
            else if (warehouseRelases.Count() != 0)
            {
                shipmentPM.WarehouseLegActualReleaseDate = null;
                shipmentPM.WarehouseLegExpectedReleaseDate = null;
            }
        }
        private void UpdateDeliveryDepartureDates()
        {
            IQueryable<WarehouseRelease> activeDeliveryWarehouseRelases = warehouseRelases.Where(r => r.StatusCode != "CARE" && r.ConnectedTo == "Delivery");
            ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(tenant);
            List<ShipmentPickUpDelivery> shipmentDeliveries = shipmentPickUpDeliveryRepository.GetShipmentDeliveryByShipmentId(shipmentPM.Id, tenant);

            IQueryable<WarehouseRelease> deliveryWarehouseReleases = null;
            shipmentDeliveries.ForEach(delivery => {
                deliveryWarehouseReleases = activeDeliveryWarehouseRelases.Where(r => r.ChildEntityReference == delivery.PickUpDeliveryNumber);
                if (deliveryWarehouseReleases.Count() == 1)
                {
                    WarehouseRelease warehouseRelease = deliveryWarehouseReleases.FirstOrDefault();
                    delivery.ATD = warehouseRelease!=null? warehouseRelease.ActualReleaseDate:null;
                    delivery.ETD = warehouseRelease != null ? warehouseRelease.ExpectedReleaseDate:null;
                    shipmentPickUpDeliveryRepository.Update(delivery);
                }
            });

            shipmentPickUpDeliveryRepository.SubmitChanges();

        }
        private void UpdateCrossDockReleaseStatus()
        {
            if (shipmentPM.MainCarriageATD == null) ChangeCrossDockReleaseStatus("RELE", "CREA");
            else 
            {
                EntityStatus departedStatus = EntityStatusRepository.GetSingleEntityStatusByCode("SDEP", tenant, true);
                if (departedStatus != null)
                {
                    EntityStatus entityStatus = EntityStatusRepository.GetSingleEntityStatus(shipmentPM.StatusId, tenant, true);
                    if (entityStatus != null && entityStatus.StatusWeight >= departedStatus.StatusWeight) ChangeCrossDockReleaseStatus("CREA", "RELE");
                }
            }
        }
        private void ChangeCrossDockReleaseStatus(string fromStatusCode, string toStatusCode)
        {
            List<WarehouseRelease> warehouseReleasesLists = warehouseRelases.Where(r => r.StatusCode == fromStatusCode).ToList();
            foreach (WarehouseRelease item in warehouseReleasesLists)
            {
                if (IsUpdateWarehouseStatus(item, fromStatusCode)) 
                {
                    item.StatusCode = toStatusCode;
                    warehouseReleaseRepository.Update(item); 
                }
            }
        }
        private bool IsUpdateWarehouseStatus(WarehouseRelease warehouseRelease, string fromStatusCode)
        {
            return (fromStatusCode == "CREA" || (fromStatusCode == "RELE" && warehouseRelease.ActualReleaseDate == null));
        }
        private void UpdateWareHouseEntryPartners()
        {
            List<WarehouseEntry> warehouseEntryLists = null;
            string fromPortId = !string.IsNullOrEmpty(shipmentPM.MainCarriageFromPortId) ? shipmentPM.MainCarriageFromPortId : shipmentPM.FromPortId;
            string toPortId = shipmentPM.ShipmentLevelCode == "H" ? shipmentPM.MainCarriageFinalDestinationPortId : shipmentPM.FinalDistenationPortId;

            if (shipmentPM.DirectionId == "D" && shipmentPM.TransportModeId == "I")
            {
                warehouseEntryLists = warehouseEntries.Where(d => d.FromAddressId != shipmentPM.MainCarriageFromAddressId || d.ToAddressId != shipmentPM.MainCarriageToAddressId || d.FromPartnerId != shipmentPM.MainCarriageFromPartnerId || d.ToAddressId != shipmentPM.MainCarriageToPartnerId).ToList();
            }
            else warehouseEntryLists = warehouseEntries.Where(d => d.FromPortId != fromPortId || d.ToPortId != toPortId).ToList();

            if (warehouseEntryLists != null && warehouseEntryLists.Count > 0)
            {
                foreach (WarehouseEntry warehouseEntry in warehouseEntryLists)
                {
                    if (shipmentPM.DirectionId == "D" && shipmentPM.TransportModeId == "I")
                    {
                        warehouseEntry.FromAddressId = shipmentPM.MainCarriageFromAddressId;
                        warehouseEntry.ToAddressId = shipmentPM.MainCarriageToAddressId;
                        warehouseEntry.FromPartnerId = shipmentPM.MainCarriageFromPartnerId;
                        warehouseEntry.ToPartnerId = shipmentPM.MainCarriageToPartnerId;
                    }
                    else
                    {
                        warehouseEntry.FromPortId = fromPortId;
                        warehouseEntry.ToPortId = toPortId;
                    }

                    warehouseEntryRepository.Update(warehouseEntry);
                }
            }
        }
        private void ConnectWarehouseReleaseToShipment()
        {
            if (!string.IsNullOrEmpty(shipmentPM.WarehouseReleasesIds))
            {
                List<string> warehouseReleasesIdLists = shipmentPM.WarehouseReleasesIds.Split(',').ToList();
                if (warehouseReleasesIdLists.Count > 0)
                {
                    List<WarehouseRelease> warehouseReleasesLists = warehouseReleaseRepository.GetAll(tenant).Where(d => warehouseReleasesIdLists.Contains(d.Id)).ToList();
                    foreach (WarehouseRelease item in warehouseReleasesLists)
                    {
                        item.IsUsed = true;
                        if (string.IsNullOrEmpty(item.ShipmentId)) item.ShipmentId = shipmentPM.Id;
                        warehouseReleaseRepository.Update(item);
                    }
                }
            }
            shipmentPM.WarehouseReleasesIds = null;
        }

      
    }
}
