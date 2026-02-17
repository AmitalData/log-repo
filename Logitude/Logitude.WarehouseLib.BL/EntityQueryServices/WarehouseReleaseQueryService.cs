using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.BL.Helpers;
using Logitude.WarehouseLib.Data;
using Logitude.WarehouseLib.Data.EntityKeys;
using Logitude.WarehouseLib.Data.EntityLists;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.WarehouseLib.BL.EntityQueryServices
{
    public partial class WarehouseReleaseQueryService
    {


        public List<WarehouseReleaseList> GetWarehouseReleaseListsByshipmentId(string shipmentId, int tenant)
        {

            List<WarehouseReleaseList> myResult = (from a in context.WarehouseReleases.Include("WarehouseReleaseStatus")
                                                   where a.ShipmentId == shipmentId && a.Tenant == tenant
                                                   select new WarehouseReleaseList()
                                                   {
                                                       Id = a.Id,
                                                       ReleaseNumber = a.ReleaseNumber,
                                                       ActualReleaseDate = a.ActualReleaseDate,
                                                       ReleaseBy = a.ReleaseBy,
                                                       StatusName = a.WarehouseReleaseStatus != null ? a.WarehouseReleaseStatus.Name : "",
                                                       ExpectedReleaseDate = a.ExpectedReleaseDate,
                                                       CreateDate = a.CreateDate,

                                                   }).ToList();
            return myResult;
        }

        public IQueryable<WarehouseReleaseList> GetWarehouseReleaseListsByTenant(int tenant)
        {

            IQueryable<WarehouseReleaseList> myResult = (from a in context.WarehouseReleases
                                                       where a.Tenant == tenant
                                                       select new WarehouseReleaseList()
                                                       {
                                                           Id = a.Id,
                                                           ReleaseNumber = a.ReleaseNumber,
                                                           ActualReleaseDate = a.ActualReleaseDate,
                                                           ReleaseBy = a.ReleaseBy,
                                                           StatusName = a.WarehouseReleaseStatus != null ? a.WarehouseReleaseStatus.Name : "",
                                                           ExpectedReleaseDate = a.ExpectedReleaseDate,
                                                           CreateDate = a.CreateDate,
                                                           StatusCode = a.StatusCode,
                                                           DirectionId = a.DirectionId,
                                                           TransportModeId = a.TransportModeId,
                                                       });
            return myResult;
        }

        public List<string> GetWarehouseReleaseListsIdsByshipmentId(string shipmentId, int tenant)
        {

            List<string> myResult = (from a in context.WarehouseReleases
                                               where a.ShipmentId == shipmentId && a.Tenant == tenant && a.StatusCode != "CARE"
                                     select a.Id).ToList();
            return myResult;
        }

        public bool CheckIfShipmentHasReleasePackage(string shipmentId, int tenant)
        {

           bool myResult = (from a in context.WarehouseReleases where a.ShipmentId == shipmentId && a.Tenant == tenant && a.StatusCode!= "CARE" select a).Any();

            return myResult;
        }

        public override void GetComposition(EntityKeyFields entityKeys, WarehouseReleasePM entityPM)
      {
          IWarehouseContext context = MainContext as IWarehouseContext;
          WarehouseReleaseKeys warehouseReleaseKeys = entityKeys as WarehouseReleaseKeys;

            WarehouseReleasePackageQueryService queryService = new WarehouseReleasePackageQueryService(context);
            entityPM.WarehouseReleasePackages = queryService.GetMulti(warehouseReleaseKeys, true);
            PackageTypeRepository packageTypeRepository = new PackageTypeRepository(entityPM.Tenant);
            foreach (WarehouseReleasePackagePM item in entityPM.WarehouseReleasePackages)
            {
                PackageType packageType = packageTypeRepository.GetSinglePackageType(item.PackageTypeId, item.Tenant);
                if (packageType != null) item.PackageTypeName = packageType.EnglishName;


                item.Dimensions = item.IsContainer ? "" : item.Length + "-" + item.Width + "-" + item.Height;

                if (!string.IsNullOrEmpty(item.ContainerNumber))
                {
                    item.ContainerNumberWarning = ContainerNumberWarehouseValidator.Validate(item.ContainerNumber);
                }

                item.VolumeUnitCode = entityPM.VolumeUnitCode;
                item.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
                item.DimensionUnitCode = entityPM.DimensionsUnitCode;
                item.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;

            }


        }


        public List<WarehouseReleaseList> GetRecentWarehouseReleasesListsByTenant(string userId, string objectTableId, int tenant)
        {

            List<WarehouseReleaseList> warehouseReleaseLists = new List<WarehouseReleaseList>();

            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }

            WarehouseReleaseRepository repository = new WarehouseReleaseRepository(tenant);
            List<WarehouseRelease> warehouseReleases = repository.GetWarehouseReleasesFromIdList(ids, tenant).ToList();

            warehouseReleases = BranchPermitionsFilter.AddUserBranchRestrictionFilters<WarehouseRelease>(new QueryOperations(), warehouseReleases.AsQueryable<WarehouseRelease>(), tenant).ToList();
            warehouseReleases = ProductPermitionsFilter.AddUserProductRestrictionFilters<WarehouseRelease>(new QueryOperations(), warehouseReleases.AsQueryable<WarehouseRelease>(), tenant).ToList();

            warehouseReleaseLists = BranchPermitionsFilter.AddUserBranchRestrictionFilters<WarehouseReleaseList>(new QueryOperations(), warehouseReleaseLists.AsQueryable<WarehouseReleaseList>(), tenant).ToList();
            warehouseReleaseLists = ProductPermitionsFilter.AddUserProductRestrictionFilters<WarehouseReleaseList>(new QueryOperations(), warehouseReleaseLists.AsQueryable<WarehouseReleaseList>(), tenant).ToList();

            if (warehouseReleases.Count > 0)
            {
                List<string> cardIds = new List<string>();
                List<string> portIds = new List<string>();
                List<string> addressIds = new List<string>();
                List<string> shipmentIds = new List<string>();
                foreach (WarehouseRelease warehouseRelease in warehouseReleases)
                {
                    if (!string.IsNullOrEmpty(warehouseRelease.WarehouseId) && !cardIds.Contains(warehouseRelease.WarehouseId)) cardIds.Add(warehouseRelease.WarehouseId);
                    if (!string.IsNullOrEmpty(warehouseRelease.CustomerId) && !cardIds.Contains(warehouseRelease.CustomerId)) cardIds.Add(warehouseRelease.CustomerId);
                    if (!string.IsNullOrEmpty(warehouseRelease.ShipmentId) && !shipmentIds.Contains(warehouseRelease.ShipmentId)) shipmentIds.Add(warehouseRelease.ShipmentId);

                }


                List<CardList> cardLists = new List<CardList>();
                if (cardIds.Count > 0)
                {
                    CardQuery cardQuery = new CardQuery(tenant);
                    cardLists = cardQuery.GetCardListsByListIds(cardIds, tenant);
                }

                List<ShipmentDataView> shipmentDataViews = new List<ShipmentDataView>();
                if (shipmentIds.Count > 0)
                {
                    ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                    shipmentDataViews = shipmentRepository.GetShipmentsFromIdList(shipmentIds, tenant);
                }

                foreach (EntityLastActivity activity in lastActivities)
                {
                    WarehouseRelease warehouseRelease = warehouseReleases.Where(d => d.Id == activity.EntityId).FirstOrDefault();
                    if (warehouseRelease != null)
                    {
                        ShipmentDataView shipmentDataView = shipmentDataViews.Where(d => d.Id == warehouseRelease.ShipmentId).FirstOrDefault();
                        var isInlandDomestic = shipmentDataView != null && shipmentDataView.TransportModeId == "I" && shipmentDataView.DirectionId == "D" ? true : false;
         
                        var warehouseReleaseList = new WarehouseReleaseList()
                        {
                            Id = warehouseRelease.Id,
                            Tenant = warehouseRelease.Tenant,
                            ReleaseNumber = warehouseRelease.ReleaseNumber,
                            TransportModeId = warehouseRelease.TransportModeId,
                            DirectionId = shipmentDataView.DirectionId,
                            StatusName = warehouseRelease.WarehouseReleaseStatus != null ? warehouseRelease.WarehouseReleaseStatus.Name : "",
                            WarehouseId = warehouseRelease.WarehouseId,
                            CustomerId = warehouseRelease.CustomerId,
                            CreateDate = warehouseRelease.CreateDate,
                            ActivityDate = activity.ActivityDate,
                            ActivityTypeName = activity.ActivityType != null ? activity.ActivityType.Name : "",
                            ActivityByUserName = activity.User != null ? activity.User.Contact.EnglishName : "",
                            DirectionName = shipmentDataView != null ? shipmentDataView.DirectionName : "",
                            TransportModeName = shipmentDataView != null ? shipmentDataView.TransportModeName : "",
                            Routing = shipmentDataView != null ? shipmentDataView.Routing : "",
                        };

                        #region Full Other Prop
                        if (!string.IsNullOrEmpty(warehouseRelease.WarehouseId))
                        {
                            CardList cardList = cardLists.Where(d => d.Id == warehouseRelease.WarehouseId).FirstOrDefault();
                            if (cardList != null) warehouseReleaseList.WarehouseName = cardList.EnglishName;

                        }


                        if (!string.IsNullOrEmpty(warehouseRelease.CustomerId))
                        {
                            CardList cardList = cardLists.Where(d => d.Id == warehouseRelease.CustomerId).FirstOrDefault();
                            if (cardList != null) warehouseReleaseList.CustomerName = cardList.EnglishName;

                        }

                        #endregion

                        warehouseReleaseLists.Add(warehouseReleaseList);
                    }
                }


            }

            return warehouseReleaseLists;
        }



    }
}
