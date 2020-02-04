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

            bool myResult = (from a in context.WarehouseReleases where a.ShipmentId == shipmentId && a.Tenant == tenant && a.StatusCode != "CARE" select a).Any();

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

            List<string> entityIds = lastActivities.Select(d => d.EntityId).ToList();

            WarehouseReleaseRepository repository = new WarehouseReleaseRepository(tenant);
            List<WarehouseRelease> warehouseReleases = repository.GetWarehouseReleasesFromIdList(entityIds, tenant).ToList();

            warehouseReleases = BranchPermitionsFilter.AddUserBranchRestrictionFilters<WarehouseRelease>(new QueryOperations(), warehouseReleases.AsQueryable<WarehouseRelease>(), tenant).ToList();
            warehouseReleases = ProductPermitionsFilter.AddUserProductRestrictionFilters<WarehouseRelease>(new QueryOperations(), warehouseReleases.AsQueryable<WarehouseRelease>(), tenant).ToList();

            warehouseReleaseLists = BranchPermitionsFilter.AddUserBranchRestrictionFilters<WarehouseReleaseList>(new QueryOperations(), warehouseReleaseLists.AsQueryable<WarehouseReleaseList>(), tenant).ToList();
            warehouseReleaseLists = ProductPermitionsFilter.AddUserProductRestrictionFilters<WarehouseReleaseList>(new QueryOperations(), warehouseReleaseLists.AsQueryable<WarehouseReleaseList>(), tenant).ToList();

            if (warehouseReleases.Count > 0)
            {
                List<string> cardIds = new List<string>();
                List<string> portIds = new List<string>();
                List<string> addressIds = new List<string>();

                foreach (WarehouseRelease warehouseRelease in warehouseReleases)
                {
                    if (!string.IsNullOrEmpty(warehouseRelease.WarehouseId) && !cardIds.Contains(warehouseRelease.WarehouseId)) cardIds.Add(warehouseRelease.WarehouseId);
                    if (!string.IsNullOrEmpty(warehouseRelease.CustomerId) && !cardIds.Contains(warehouseRelease.CustomerId)) cardIds.Add(warehouseRelease.CustomerId);
                    if (!string.IsNullOrEmpty(warehouseRelease.FromPortId) && !cardIds.Contains(warehouseRelease.FromPortId)) cardIds.Add(warehouseRelease.FromPortId); //  From Port Is Warehouse Cards
                    if (!string.IsNullOrEmpty(warehouseRelease.ToPortId) && !portIds.Contains(warehouseRelease.ToPortId)) portIds.Add(warehouseRelease.ToPortId);
                }

                List<CardList> cardLists = new List<CardList>();
                if (cardIds.Count > 0)
                {
                    CardQuery cardQuery = new CardQuery(tenant);
                    cardLists = cardQuery.GetCardListsByListIds(cardIds, tenant);
                }
                List<PortList> portLists = new List<PortList>();
                if (portIds.Count > 0)
                {
                    PortQuery portQuery = new PortQuery(tenant);
                    portLists = portQuery.GetPortListsByListIds(portIds, tenant);
                }

                foreach (EntityLastActivity activity in lastActivities)
                {
                    WarehouseRelease warehouseRelease = warehouseReleases.Where(d => d.Id == activity.EntityId).FirstOrDefault();
                    if (warehouseRelease != null)
                    {
                        var warehouseReleaseList = new WarehouseReleaseList()
                        {
                            Id = warehouseRelease.Id,
                            Tenant = warehouseRelease.Tenant,
                            ReleaseNumber = warehouseRelease.ReleaseNumber,
                            TransportModeId = warehouseRelease.TransportModeId,
                            DirectionId = warehouseRelease.DirectionId,
                            StatusName = warehouseRelease.WarehouseReleaseStatus != null ? warehouseRelease.WarehouseReleaseStatus.Name : "",
                            WarehouseId = warehouseRelease.WarehouseId,
                            CustomerId = warehouseRelease.CustomerId,
                            CreateDate = warehouseRelease.CreateDate,
                            ActivityDate = activity.ActivityDate,
                            ActivityTypeName = activity.ActivityType != null ? activity.ActivityType.Name : "",
                            ActivityByUserName = activity.User != null ? activity.User.Contact.EnglishName : "",
                            DirectionName = warehouseRelease.Direction != null ? warehouseRelease.Direction.Name : "",
                            TransportModeName = warehouseRelease.TransportMode != null ? warehouseRelease.TransportMode.Name : "",

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

                        warehouseReleaseList.Routing = ComputeFieldRouting(portLists, cardLists, warehouseRelease);

                        #endregion

                        warehouseReleaseLists.Add(warehouseReleaseList);
                    }
                }


            }

            return warehouseReleaseLists;
        }

        private string ComputeFieldRouting(List<PortList> portLists, List<CardList> cardLists , WarehouseRelease warehouseRelease)
        {
            string routing = (GetEnglishNameByCardId(cardLists, warehouseRelease.FromPortId) + " > ");
            if (warehouseRelease.ToTypeCode == "PORT" && !string.IsNullOrEmpty(warehouseRelease.ToPortId))
            {
                routing += GetPortCodeFromPortId(portLists, warehouseRelease.ToPortId);
            }
            else if (warehouseRelease.ToTypeCode == "PART" && !string.IsNullOrEmpty(warehouseRelease.ToAddressId))
            {
                routing += warehouseRelease.ToAddress != null ? warehouseRelease.ToAddress.City + " " : "";
                routing += warehouseRelease.ToAddress != null && warehouseRelease.ToAddress.Country != null ? warehouseRelease.ToAddress.Country.EnglishName : "";
            }
            else if (warehouseRelease.ToTypeCode == "CASL")
            {
                routing += (warehouseRelease.ToAddressCity) + " " + (warehouseRelease.ToAddressCountry != null ? warehouseRelease.ToAddressCountry.EnglishName : "");
            }
            return routing;
        }

        private string GetPortCodeFromPortId(List<PortList> portLists, string portId)
        {
            string portCode = string.Empty;
            if (!string.IsNullOrEmpty(portId))
            {
                PortList fromPort = portLists.Where(d => d.Id == portId).FirstOrDefault();
                if (fromPort != null) portCode = fromPort.Code;
            }
            return portCode;
        }

        private string GetEnglishNameByCardId(List<CardList> cardLists, string cardId)
        {
            string englishName = string.Empty;
            if (!string.IsNullOrEmpty(cardId))
            {
                CardList cardList = cardLists.Where(d => d.Id == cardId).FirstOrDefault();
                if (cardList != null) englishName = cardList.EnglishName;
            }
            return englishName;
        }



        public List<WarehouseReleaseList> GetWarehouseReleasesByEntryId(string entityId, int tenant)
        {
            List<WarehouseReleaseList> warehouseReleaseLists = null;
            List<string> warehouseEntryPackagesIds = (from a in context.WarehouseEntryPackages where a.Tenant == tenant && a.WarehouseEntryId == entityId select a.Id).ToList();
            if (warehouseEntryPackagesIds.Count > 0)
            {
                List<string> warehouseReleasePackagesIds = (from a in context.WarehouseEntryPackagesReleases where a.Tenant == tenant && warehouseEntryPackagesIds.Contains(a.EntryPackageId) select a.ReleasePackageId).ToList();
                if (warehouseReleasePackagesIds.Count > 0)
                {
                    List<string> warehouseReleaseIds = (from a in context.WarehouseReleasePackages where a.Tenant == tenant && warehouseReleasePackagesIds.Contains(a.Id) select a.WarehouseReleaseId).ToList();
                    warehouseReleaseLists = (from a in context.WarehouseReleases.Include("WarehouseReleaseStatus")
                                             where a.Tenant == tenant && warehouseReleaseIds.Contains(a.Id)
                                             select new WarehouseReleaseList()
                                             {
                                                 ActualReleaseDate = a.ActualReleaseDate,
                                                 ExpectedReleaseDate = a.ExpectedReleaseDate,
                                                 ReleaseNumber = a.ReleaseNumber,
                                                 Id = a.Id,
                                                 ReleaseDate = a.ActualReleaseDate == null ? a.ExpectedReleaseDate : a.ActualReleaseDate,
                                                 ShipmentNumber = a.ShipmentNumber,
                                                 StatusName = a.WarehouseReleaseStatus != null ? a.WarehouseReleaseStatus.Name : null,
                                                 ConnectedTo = a.ConnectedTo,

                                             }).ToList();


                }
            }



            return warehouseReleaseLists;
        }


        public List<WarehouseReleasePM> GetWarehouseReleaseListsByCustomerIdAndWarehouseId(string customerId, string warehouseId, int tenant)
        {
            List<WarehouseReleasePM> myResult = (from a in context.WarehouseReleases.Include("ToPort").Include("ToAddress.Country").Include("ToAddressCountry")
                                                 where a.WarehouseId == warehouseId && a.CustomerId == customerId && a.Tenant == tenant && !a.IsUsed && a.StatusCode == "CREA"
                                                 select new WarehouseReleasePM()
                                                 {
                                                     Id = a.Id,
                                                     ReleaseNumber = a.ReleaseNumber,
                                                     ActualReleaseDate = a.ActualReleaseDate,
                                                     ReleaseBy = a.ReleaseBy,
                                                     StatusName = a.WarehouseReleaseStatus != null ? a.WarehouseReleaseStatus.Name : "",
                                                     ExpectedReleaseDate = a.ExpectedReleaseDate,
                                                     CreateDate = a.CreateDate,
                                                     ShipmentId = a.ShipmentId,
                                                     References = a.CustomerRef1 + (!string.IsNullOrEmpty(a.CustomerRef1) && !string.IsNullOrEmpty(a.CustomerRef1) ? "," : "") + a.CustomerRef2,
                                                     Destination = a.ToTypeCode == "PORT" ? a.ToPort!=null? a.ToPort.Code:"" : a.ToTypeCode == "PART" ? (a.ToAddress!=null?a.ToAddress.City + " " :"") +  (a.ToAddress != null && a.ToAddress.Country!=null ? a.ToAddress.Country.EnglishName : "") : a.ToTypeCode == "CASL" ? (a.ToAddressCity) + " " + (a.ToAddressCountry != null ? a.ToAddressCountry.EnglishName : "") : "",
                                                 }).ToList();
            WarehouseReleasePackageQueryService warehouseReleasePackageQueryService = new WarehouseReleasePackageQueryService(tenant);
            foreach (WarehouseReleasePM item in myResult)
            {
                item.WarehouseReleasePackages = warehouseReleasePackageQueryService.GetWarehouseReleasePackagePMListsByWarehouseReleaseId(item.Id, tenant);
                
            }
            return myResult;
        }

    }
}
