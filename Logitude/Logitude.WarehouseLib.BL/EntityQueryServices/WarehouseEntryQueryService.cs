using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.BL.Helpers;
using Logitude.WarehouseLib.BL.Service;
using Logitude.WarehouseLib.Data;
using Logitude.WarehouseLib.Data.EntityKeys;
using Logitude.WarehouseLib.Data.EntityLists;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.WarehouseLib.BL.EntityQueryServices
{



    public partial  class WarehouseEntryQueryService
    {


        public IQueryable<WarehouseEntryList> GetWarehouseEntryListsByTenant( int tenant)
        {

            IQueryable<WarehouseEntryList> myResult = (from a in context.WarehouseEntries
                                                       where a.Tenant == tenant
                                                       select new WarehouseEntryList()
                                                       {
                                                           Id = a.Id,
                                                           EntryNumber = a.EntryNumber,
                                                           CreateDate = a.CreateDate,
                                                           DirectionId = a.DirectionId,
                                                           TransportModeId = a.TransportModeId,
                                                           ShipmentId = a.ShipmentId,
                                                           StatusCode = a.StatusCode,
                                                           ConnectedToShipment = a.ConnectedToShipment,
                                                       });
            return myResult;
        }



        public List<WarehouseEntryList> GetWarehouseEntryListsByshipmentId(string shipmentId, int tenant)
        {

            List<WarehouseEntryList> myResult = (from a in context.WarehouseEntries
                                                    where a.ShipmentId == shipmentId && a.Tenant == tenant
                                                      select new WarehouseEntryList()
                                                    {
                                                        Id = a.Id,
                                                        EntryNumber = a.EntryNumber,
                                                        CreateDate = a.CreateDate,
                                                      }).ToList();
            return myResult;
        }



        public override void GetComposition(EntityKeyFields entityKeys, WarehouseEntryPM entityPM)
        {
            IWarehouseContext context = MainContext as IWarehouseContext;
            WarehouseEntryKeys warehouseEntryKeys = entityKeys as WarehouseEntryKeys;

            WarehouseEntryPackageQueryService queryService = new WarehouseEntryPackageQueryService(context);
            entityPM.WarehouseEntryPackages = queryService.GetMulti(warehouseEntryKeys, true);
            PackageTypeRepository packageTypeRepository = new PackageTypeRepository(entityPM.Tenant);

            #region Related Releases
            List<string> warehouseEntryPackagesIds = entityPM.WarehouseEntryPackages.Where(d=>d.Instock!=d.Quantity).Select(d => d.Id).ToList();
            List<WarehouseReleasePackageList> warehouseReleasePackagesLists = null;
            List<WarehouseEntryPackagesReleaseList> warehouseEntryPackagesReleaseLists = null;

            if (warehouseEntryPackagesIds.Count > 0)
            {
                WarehouseEntryPackagesReleaseQueryService warehouseEntryPackagesReleaseQueryService = new WarehouseEntryPackagesReleaseQueryService(entityPM.Tenant);
                warehouseEntryPackagesReleaseLists = warehouseEntryPackagesReleaseQueryService.GetWarehouseEntryPackagePMListsByCustomerIdIdAndWarehouseId(warehouseEntryPackagesIds, entityPM.Tenant);
                if (warehouseEntryPackagesReleaseLists.Count > 0)
                {
                    WarehouseReleasePackageQueryService warehouseReleasePackageQueryService = new WarehouseReleasePackageQueryService(entityPM.Tenant);
                    warehouseReleasePackagesLists = warehouseReleasePackageQueryService.GetWarehouseReleasePackageListsByIds(warehouseEntryPackagesReleaseLists.Select(d => d.ReleasePackageId).ToList(), entityPM.Tenant);
                }
            }
            #endregion

            foreach (WarehouseEntryPackagePM item in entityPM.WarehouseEntryPackages)
            {
                PackageType packageType = packageTypeRepository.GetSinglePackageType(item.PackageTypeId, item.Tenant);
                if (packageType != null) item.PackageTypeName = packageType.EnglishName;
                item.Dimensions = item.IsContainer ? "" : item.Length + "-" + item.Width + "-" + item.Height;

                if (!string.IsNullOrEmpty(item.ContainerNumber))
                {  item.ContainerNumberWarning  = ContainerNumberWarehouseValidator.Validate(item.ContainerNumber);
                }
                item.VolumeUnitCode = entityPM.VolumeUnitCode;
                item.GrossWeightUnitCode = entityPM.GrossWeightUnitCode;
                item.DimensionUnitCode = entityPM.DimensionsUnitCode;
                item.ChargeableWeightUnitCode = entityPM.ChargeableWeightUnitCode;

                #region Related Releases
                if (warehouseEntryPackagesReleaseLists != null && warehouseReleasePackagesLists != null)
                {
                    var releasePackageIds = warehouseEntryPackagesReleaseLists.Where(d => d.EntryPackageId == item.Id).Select(d => d.ReleasePackageId).ToList();
                    if (releasePackageIds.Count > 0)
                    {
                        var lists = warehouseReleasePackagesLists.Where(d => releasePackageIds.Contains(d.Id)).GroupBy(d => d.ReleaseNumber).Select(d => d.FirstOrDefault().ReleaseNumber).ToList();
                        foreach (string releaseNumber in lists)
                        {
                            if (string.IsNullOrEmpty(item.ReleasesNumber)) item.ReleasesNumber = releaseNumber;
                            else item.ReleasesNumber += ("," + releaseNumber);
                        }
                    }
                }

                #endregion

            }

        }


        public List<WarehouseEntryList> GetRecentWarehouseEntriesListsByTenant(string userId, string objectTableId, int tenant)
        {

            List<WarehouseEntryList> warehouseEntryLists = new List<WarehouseEntryList>();

            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }
       
            
            WarehouseEntryRepository repository = new WarehouseEntryRepository(tenant);
            List<WarehouseEntry> warehouseEntries = repository.GetWarehouseEntriesFromIdList(ids, tenant).ToList();

            warehouseEntries = BranchPermitionsFilter.AddUserBranchRestrictionFilters<WarehouseEntry>(new QueryOperations(), warehouseEntries.AsQueryable<WarehouseEntry>(), tenant).ToList();
            warehouseEntries = ProductPermitionsFilter.AddUserProductRestrictionFilters<WarehouseEntry>(new QueryOperations(), warehouseEntries.AsQueryable<WarehouseEntry>(), tenant).ToList();

            warehouseEntryLists = BranchPermitionsFilter.AddUserBranchRestrictionFilters<WarehouseEntryList>(new QueryOperations(), warehouseEntryLists.AsQueryable<WarehouseEntryList>(), tenant).ToList();
            warehouseEntryLists = ProductPermitionsFilter.AddUserProductRestrictionFilters<WarehouseEntryList>(new QueryOperations(), warehouseEntryLists.AsQueryable<WarehouseEntryList>(), tenant).ToList();


            if (warehouseEntries.Count > 0)
            {
                List<string> cardIds = new List<string>();
                foreach (WarehouseEntry warehouseEntry in warehouseEntries)
                {
                    var isInlandDomestic = warehouseEntry.TransportModeId == "I" && warehouseEntry.DirectionId == "D" ? true : false;
                    if (!string.IsNullOrEmpty(warehouseEntry.WarehouseId) && !cardIds.Contains(warehouseEntry.WarehouseId)) cardIds.Add(warehouseEntry.WarehouseId);
                    if (!string.IsNullOrEmpty(warehouseEntry.CustomerId) && !cardIds.Contains(warehouseEntry.CustomerId)) cardIds.Add(warehouseEntry.CustomerId);
                }


                List<CardList> cardLists = new List<CardList>();
                if (cardIds.Count > 0)
                {
                    CardQuery cardQuery = new CardQuery(tenant);
                    cardLists = cardQuery.GetCardListsByListIds(cardIds, tenant);
                }

                foreach (EntityLastActivity activity in lastActivities)
                {
                    WarehouseEntry warehouseEntry = warehouseEntries.Where(d => d.Id == activity.EntityId).FirstOrDefault();
                    if (warehouseEntry != null)
                    {
                        var isInlandDomestic = warehouseEntry.TransportModeId == "I" && warehouseEntry.DirectionId == "D" ? true : false;
                        var warehouseEntryList = new WarehouseEntryList()
                        {
                            Id = warehouseEntry.Id,
                            Tenant = warehouseEntry.Tenant,
                            EntryNumber = warehouseEntry.EntryNumber,
                            EntryReference = warehouseEntry.EntryReference,
                            TransportModeId = warehouseEntry.TransportModeId,
                            DirectionId = warehouseEntry.DirectionId,
                            StatusName = warehouseEntry.WarehouseEntryStatus != null ? warehouseEntry.WarehouseEntryStatus.Name : "",
                            WarehouseId = warehouseEntry.WarehouseId,
                            CustomerId = warehouseEntry.CustomerId,
                            ToPortId = warehouseEntry.ToPortId,
                            FromPortId = warehouseEntry.FromPortId,
                            CreateDate = warehouseEntry.CreateDate,
                            ActivityDate = activity.ActivityDate,
                            ActivityTypeName = activity.ActivityType!=null? activity.ActivityType.Name:"",
                            ActivityByUserName = activity.User!=null? activity.User.Contact.EnglishName:"",
                            DirectionName = warehouseEntry.Direction != null ? warehouseEntry.Direction.Name : "",
                            TransportModeName = warehouseEntry.TransportMode != null ? warehouseEntry.TransportMode.Name : "",
                        };

                        #region Full Other Prop
                        if (!string.IsNullOrEmpty(warehouseEntry.WarehouseId))
                        {
                            CardList cardList = cardLists.Where(d => d.Id == warehouseEntry.WarehouseId).FirstOrDefault();
                            if (cardList != null) warehouseEntryList.WarehouseName = cardList.EnglishName;

                        }


                        if (!string.IsNullOrEmpty(warehouseEntry.CustomerId))
                        {
                            CardList cardList = cardLists.Where(d => d.Id == warehouseEntry.CustomerId).FirstOrDefault();
                            if (cardList != null) warehouseEntryList.CustomerName = cardList.EnglishName;

                        }


                        #region Routing
                        WarehouseEntryRoutingService warehouseEntryRoutingService = new WarehouseEntryRoutingService();
                        WarehouseEntryRouting warehouseEntryRouting = warehouseEntryRoutingService.GetWarehouseEntryRouting(new WarehouseEntryRoutingArgs() { TransportModeId = warehouseEntry.TransportModeId, DirectionId = warehouseEntry.DirectionId, FromAddressId = warehouseEntry.FromAddressId, ToAddressId = warehouseEntry.ToAddressId, FromPortId = warehouseEntry.FromPortId, ToPortId = warehouseEntry.ToPortId, ToCountryId = warehouseEntry.ToCountryId, FromCountryId = warehouseEntry.FromCountryId, FromTypeCode = warehouseEntry.FromTypeCode, ToTypeCode = warehouseEntry.ToTypeCode, Tenant = warehouseEntry.Tenant });
                        warehouseEntryList.Origin = warehouseEntryRouting.Origin;
                        warehouseEntryList.Destination = warehouseEntryRouting.Destination;
                        warehouseEntryList.Routing = warehouseEntryRouting.Routing;
                        #endregion

                        #endregion


                        warehouseEntryLists.Add(warehouseEntryList);
                    }
                }


               

            }





            return warehouseEntryLists;
        }

    }

}
