using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.BL.EntityQueryServices;
using Logitude.WarehouseLib.BL.EntityUpdateServices;
using Logitude.WarehouseLib.Data;
using Logitude.WarehouseLib.Data.EntityLists;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
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
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.WarehouseModel.Extended
{
    public class WarehouseReleaseExtendedController : ApiController
    {
        public HttpResponseMessage PostWarehouseReleasePM(WarehouseReleasePM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.CheckContactFeature("WarehouseRelease", "NEW", authToken.Tenant);

                        WarehouseEntryPackageRepository warehouseEntryPackageRepository = new WarehouseEntryPackageRepository(entityPM.Tenant);
                        WarehouseEntryPackagesReleaseRepository warehouseEntryPackagesReleaseRepository = new WarehouseEntryPackagesReleaseRepository(entityPM.Tenant);

                        IWarehouseContext MyContext = WarehouseContext.GetContext(entityPM.Tenant);
                        WarehouseReleaseUpdateService service = new WarehouseReleaseUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);

                        List<string> entryPackageIds = entityPM.WarehouseReleasePackages.Select(d => d.EntryPackageId).ToList();
                        List<WarehouseEntryPackage> warehouseEntryPackages = warehouseEntryPackageRepository.GetWarehouseEntryPackageByIds(entryPackageIds, entityPM.Tenant);

                        entityPM.TotalQuantity = entityPM.WarehouseReleasePackages.Sum(a => a.Quantity);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        service.Update(entityPM, true);

                        foreach (WarehouseReleasePackagePM item in entityPM.WarehouseReleasePackages)
                        {
                            WarehouseEntryPackage warehouseEntryPackage = warehouseEntryPackages.Where(d => d.Id == item.EntryPackageId).FirstOrDefault();
                            if (warehouseEntryPackage != null)
                            {
                                warehouseEntryPackage.Instock -= item.Quantity;
                                warehouseEntryPackageRepository.Update(warehouseEntryPackage);
                            }

                            WarehouseEntryPackagesRelease warehouseEntryPackagesRelease = new WarehouseEntryPackagesRelease()
                            {
                                CreatedByUserId = entityPM.CreatedByUserId,
                                UpdatedByUserId = entityPM.UpdatedByUserId,
                                CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                                UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                                Tenant = item.Tenant,
                                Quantity = item.Quantity,
                                ReleasePackageId = item.Id,
                                EntryPackageId = warehouseEntryPackage.Id,

                            };
                            warehouseEntryPackagesReleaseRepository.Add(warehouseEntryPackagesRelease);

                        }

                        warehouseEntryPackageRepository.SubmitChanges();
                        warehouseEntryPackagesReleaseRepository.SubmitChanges();

                        //this.UpdateShipment(entityPM);

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }

        private void UpdateShipment(WarehouseReleasePM entityPM)
        {
            if (!string.IsNullOrEmpty(entityPM.ShipmentId))
            {
                ShipmentRepository myShipmentRepository = new ShipmentRepository(entityPM.Tenant);
                Shipment myShipment = myShipmentRepository.GetSingleShipment(entityPM.ShipmentId, entityPM.Tenant);
                if (myShipment != null && string.IsNullOrEmpty(myShipment.WarehouseLegWarehouseId))
                {
                    myShipment.WarehouseLegWarehouseId = entityPM.WarehouseId;
                    myShipment.WarehouseLegExpectedReleaseDate = entityPM.ExpectedReleaseDate;
                    myShipment.WarehouseLegActualReleaseDate = entityPM.ActualReleaseDate;
                    myShipmentRepository.Update(myShipment);
                    myShipmentRepository.SubmitChanges();
                }
            }
        }

        public HttpResponseMessage GetWarehouseReleaseListsByShipmentId(string shipmentId ,int tenant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                        WarehouseReleaseQueryService warehouseReleaseQueryService = new WarehouseReleaseQueryService(tenant);
                        List<WarehouseReleaseList> warehouseReleaseLists = warehouseReleaseQueryService.GetWarehouseReleaseListsByshipmentId(shipmentId, tenant);

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, warehouseReleaseLists);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


        public HttpResponseMessage PutCancelWarehouseReleasePM(WarehouseReleasePM entityPM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        SecurityUtility.CheckContactFeature("WarehouseRelease", "UPDATE", authToken.Tenant);
                        WarehouseEntryPackagesReleaseRepository warehouseEntryPackagesReleaseRepository = null;
                        WarehouseEntryPackageRepository warehouseEntryPackageRepository = null;
                        if (entityPM.WarehouseReleasePackages != null && entityPM.WarehouseReleasePackages.Count > 0)
                        {
                            warehouseEntryPackageRepository = new WarehouseEntryPackageRepository(entityPM.Tenant);
                            warehouseEntryPackagesReleaseRepository = new WarehouseEntryPackagesReleaseRepository(entityPM.Tenant);


                            List<string> releasePackageIds = entityPM.WarehouseReleasePackages.Select(d => d.Id).ToList();
                            List<WarehouseEntryPackagesRelease> warehouseEntryPackagesReleases = warehouseEntryPackagesReleaseRepository.GetWarehouseEntryPackagesReleaseByReleasePackageIds(releasePackageIds, entityPM.Tenant);

                            List<string> entryPackageIds = warehouseEntryPackagesReleases.Select(d => d.EntryPackageId).ToList();
                            List<WarehouseEntryPackage> warehouseEntryPackages = warehouseEntryPackageRepository.GetWarehouseEntryPackageByIds(entryPackageIds, entityPM.Tenant);

                            foreach (WarehouseReleasePackagePM item in entityPM.WarehouseReleasePackages)
                            {

                                WarehouseEntryPackagesRelease warehouseEntryPackagesRelease = warehouseEntryPackagesReleases.Where(d => d.ReleasePackageId == item.Id).FirstOrDefault();

                                if (warehouseEntryPackagesRelease != null)
                                {
                                    WarehouseEntryPackage warehouseEntryPackage = warehouseEntryPackages.Where(d => d.Id == warehouseEntryPackagesRelease.EntryPackageId).FirstOrDefault();
                                    if (warehouseEntryPackage != null)
                                    {
                                        warehouseEntryPackage.Instock += item.Quantity;
                                        warehouseEntryPackageRepository.Update(warehouseEntryPackage);
                                    }

                                    item.Quantity = 0;
                                    item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;

                                    warehouseEntryPackagesRelease.IsCanceled = true;
                                    warehouseEntryPackagesReleaseRepository.Update(warehouseEntryPackagesRelease);

                                }

                            }
                            entityPM.StatusCode = "CARE";
                            entityPM.StatusName = "Cancel";

                        }

                        IWarehouseContext MyContext = WarehouseContext.GetContext(entityPM.Tenant);
                        WarehouseReleaseUpdateService service = new WarehouseReleaseUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        service.InitializeEntityPM(entityPM);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        service.Update(entityPM, true);

                        if (warehouseEntryPackageRepository!=null) warehouseEntryPackageRepository.SubmitChanges();
                        if (warehouseEntryPackagesReleaseRepository != null) warehouseEntryPackagesReleaseRepository.SubmitChanges();

                        EventTypeQuery eventTypeQuery = new EventTypeQuery(entityPM.Tenant);

                        EventTypePM eventTypePM = eventTypeQuery.GetSingleEventTypePMByCode("CARE", entityPM.Tenant);

                        if (eventTypePM != null)
                        {
                            ObjectTableQuery tablesQuery = new ObjectTableQuery(authToken.Tenant);
                            ObjectTablePM table = tablesQuery.GetObjectTableByName("WarehouseRelease", 0);
                            TraceEventRepository traceEventRepository = new TraceEventRepository(entityPM.Tenant);
                            TraceEvent newEvent = new TraceEvent()
                            {
                                Id = IdCounter.GetNumber("EventType", entityPM.Tenant).ToString(),
                                LogDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                                EventDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant),
                                Tenant = entityPM.Tenant,
                                UserId = entityPM.UpdatedByUserId,
                                ObjectTableId = table!=null ? table.Id:null,
                                EntityId = entityPM.Id,
                                IsAddedManually = false,
                                EventTypeId = eventTypePM.Id,
                            };
                            traceEventRepository.Add(newEvent);
                            traceEventRepository.SubmitChanges();
                        }
                      
                        
                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


        public HttpResponseMessage GetCrossDockWorkspaceSummary(string transportModeId, string directionId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                CrossDockWorkspaceSummaryClass crossDockWorkspaceSummaryClass = new CrossDockWorkspaceSummaryClass();

                WarehouseEntryQueryService warehouseEntryQueryService = new WarehouseEntryQueryService(authToken.Tenant);
                IQueryable<WarehouseEntryList> warehouseEntryLists = warehouseEntryQueryService.GetWarehouseEntryListsByTenant(authToken.Tenant);

                if (!string.IsNullOrEmpty(transportModeId) && transportModeId!= "All")
                {
                    warehouseEntryLists = warehouseEntryLists.Where(d => d.TransportModeId == transportModeId);
                }

                if (!string.IsNullOrEmpty(directionId) && directionId != "All")
                {
                    warehouseEntryLists = warehouseEntryLists.Where(d => d.DirectionId == directionId);
                }

                if (warehouseEntryLists != null)
                {
                    crossDockWorkspaceSummaryClass.CreatedWarehouseEntriesCount = warehouseEntryLists.Where(d =>d.StatusCode == "CREA").Count();
                    crossDockWorkspaceSummaryClass.EnteredWarehouseEntriesCount = warehouseEntryLists.Where(d =>d.StatusCode == "ENTE").Count();
                    crossDockWorkspaceSummaryClass.ConnectedToShipmentsWarehouseEntriesCount = warehouseEntryLists.Where(d=>d.ConnectedToShipment == true).Count();
                    crossDockWorkspaceSummaryClass.NotConnectedToShipmentsWarehouseEntriesCount = warehouseEntryLists.Where(d=>d.ConnectedToShipment == false).Count();
                    crossDockWorkspaceSummaryClass.AllWarehouseEntriesCount = warehouseEntryLists.Count();
                }


                WarehouseReleaseQueryService warehouseReleaseQueryService = new WarehouseReleaseQueryService(authToken.Tenant);
                IQueryable<WarehouseReleaseList> warehouseReleaseList = warehouseReleaseQueryService.GetWarehouseReleaseListsByTenant(authToken.Tenant);
                if (!string.IsNullOrEmpty(transportModeId) && transportModeId != "All")
                {
                    warehouseReleaseList = warehouseReleaseList.Where(d => d.TransportModeId == transportModeId);
                }

                if (!string.IsNullOrEmpty(directionId) && directionId != "All")
                {
                    warehouseReleaseList = warehouseReleaseList.Where(d => d.DirectionId == directionId);
                }


                if (warehouseReleaseList != null)
                {
                    crossDockWorkspaceSummaryClass.CreatedWarehouseReleasesCount = warehouseReleaseList.Where(d => d.StatusCode == "CREA").Count();
                    crossDockWorkspaceSummaryClass.WarehouseReleasedCount = warehouseReleaseList.Where(d => d.StatusCode == "RELE").Count();
                    crossDockWorkspaceSummaryClass.CanceledReleasesCount = warehouseReleaseList.Where(d => d.StatusCode == "CARE").Count();
                    crossDockWorkspaceSummaryClass.AllWarehouseReleasesCount = warehouseReleaseList.Count();
                }

                return Request.CreateResponse(HttpStatusCode.OK, crossDockWorkspaceSummaryClass);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetRecentWarehouseReleases()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("WarehouseRelease", "READ", tenant);
                ContactQuery contactQuery = new ContactQuery(tenant);
                string loggedContactId = null;
                ContactPM contact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                if (contact != null)
                {
                    loggedContactId = contact.Id;
                }

                string objectTableId = null;
                ObjectTableRepository objecttableRepository = new ObjectTableRepository(tenant);
                ObjectTable objecttable = objecttableRepository.GetObjectTableByName("WarehouseRelease", 0, true);
                if (objecttable != null)
                {
                    objectTableId = objecttable.Id;
                }

                WarehouseReleaseQueryService warehouseReleaseQueryService = new WarehouseReleaseQueryService(tenant);
                IQueryable<WarehouseReleaseList> first = warehouseReleaseQueryService.GetRecentWarehouseReleasesListsByTenant(loggedContactId, objectTableId, tenant).AsQueryable();
                IQueryable<WarehouseReleaseList> result = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), first, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }





    }

    public class CrossDockWorkspaceSummaryClass
    {
        public int AllWarehouseEntriesCount { get; set; }
        public int CreatedWarehouseEntriesCount { get; set; }
        public int EnteredWarehouseEntriesCount { get; set; }
        public int ConnectedToShipmentsWarehouseEntriesCount { get; set; }
        public int NotConnectedToShipmentsWarehouseEntriesCount { get; set; }

        public int AllWarehouseReleasesCount { get; set; }
        public int CreatedWarehouseReleasesCount { get; set; }
        public int WarehouseReleasedCount { get; set; }
        public int CanceledReleasesCount { get; set; }
    }
}