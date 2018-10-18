using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateGlobalZoneList(GlobalZoneList currentEntity)
        {
        }

        public IQueryable<GlobalZone> GetGlobalZones(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("GlobalZone", "READ", tenant);

            globalZoneRepository = new GlobalZoneRepository(tenant);
            return this.globalZoneRepository.GetGlobalZones(0);
        }

        public IQueryable<GlobalZone> GetGlobalZonesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("GlobalZone", "READ", tenant);

            globalZoneRepository = new GlobalZoneRepository(tenant);
            return this.globalZoneRepository.GetGlobalZones(tenant).Where(g => g.Tenant == tenant);
        }

        public IQueryable<GlobalZonePM> GetGlobalZonePMsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("GlobalZone", "READ", tenant);

            globalZoneQuery = new GlobalZoneQuery(tenant);
            return this.globalZoneQuery.GetGlobalZonePMsByTenant(tenant).Where(g => g.Tenant == tenant);
        }

        public GlobalZonePM GetSingleGlobalZone(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("GlobalZone", "READ", tenant);

            globalZoneQuery = new GlobalZoneQuery(tenant);
            return globalZoneQuery.GetSinglePM(id, tenant);
        }

        public GlobalZonePM GetGlobalZoneById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("GlobalZone", "READ", tenant);

            globalZoneQuery = new GlobalZoneQuery(tenant);
            GlobalZonePM zone = globalZoneQuery.GetSinglePM(id, tenant);
            return zone;
        }

        public GlobalZoneList GetSingleGlobalZoneList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("GlobalZone", "READ", tenant);

            globalZoneRepository = new GlobalZoneRepository(tenant);
            GlobalZoneList zoneList = null;
            GlobalZone zone = globalZoneRepository.GetSingleGlobalZone(id, tenant);

            if (zone != null)
            {
                List<GlobalZone> singleEntityList = new List<GlobalZone>();
                singleEntityList.Add(zone);

                globalZoneQuery = new GlobalZoneQuery(globalZoneRepository);
                IQueryable<GlobalZone> iQueryable = singleEntityList.AsQueryable();
                IQueryable<GlobalZoneList> iQueryableEntityList = globalZoneQuery.GetIQueryableEntityList(iQueryable);
                zoneList = iQueryableEntityList.FirstOrDefault();
            }
            return zoneList;
        }

        public IQueryable<GlobalZoneList> GetGlobalZoneLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("GlobalZone", "READ", tenant);

            globalZoneRepository = new GlobalZoneRepository(tenant);
            globalZoneQuery = new GlobalZoneQuery(globalZoneRepository);

            IQueryable<GlobalZone> zones = globalZoneRepository.GetGlobalZones(tenant);
            IQueryable<GlobalZoneList> query2 = globalZoneQuery.GetIQueryableEntityList(zones);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<GlobalZoneList> GetGlobalZoneFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("GlobalZone", "READ", tenant);

            globalZoneRepository = new GlobalZoneRepository(tenant);
            globalZoneQuery = new GlobalZoneQuery(globalZoneRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<GlobalZone> zones = globalZoneRepository.GetGlobalZones(tenant);
          
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            zones = filter.GetFilteredQuery<GlobalZone>(nonListQueryOperation, zones);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<GlobalZoneList> query2 = globalZoneQuery.GetIQueryableEntityList(zones);

            query2 = filter.GetFilteredQuery<GlobalZoneList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(GlobalZoneList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("GlobalZone", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<GlobalZoneList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<GlobalZoneList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<GlobalZoneList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<GlobalZoneList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<GlobalZoneList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }
            
            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetGlobalZoneFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("GlobalZone", "READ", tenant);

            globalZoneRepository = new GlobalZoneRepository(tenant);
            globalZoneQuery = new GlobalZoneQuery(globalZoneRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<GlobalZone> zones = globalZoneRepository.GetGlobalZones(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            zones = filter.GetFilteredQuery<GlobalZone>(nonListQueryOperation, zones);

            IQueryable<GlobalZoneList> query2 = globalZoneQuery.GetIQueryableEntityList(zones);

            query2 = filter.GetFilteredQuery<GlobalZoneList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public IQueryable<GlobalZonePM> GetGlobalZonesSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("GlobalZone", "READ", tenant);

            globalZoneQuery = new GlobalZoneQuery(tenant);
            IQueryable<GlobalZonePM> q = globalZoneQuery.GetGlobalZonesByCodeOrName(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public void InsertGlobalZone(GlobalZonePM entityPm)
        {
            SecurityUtility.CheckContactFeature("GlobalZone", "NEW", entityPm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }

            GlobalZoneService service = new GlobalZoneService(objectContext, entityPm.Tenant);
            service.Create(entityPm);

            TableLastUpdateClass.UpdateTableHistory(entityPm.Tenant, "GlobalZone");
        }

        public void UpdateGlobalZone(GlobalZonePM currententityPm)
        {
            SecurityUtility.CheckContactFeature("GlobalZone", "UPDATE", currententityPm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currententityPm.Tenant);
            }

            GlobalZoneService service = new GlobalZoneService(objectContext, currententityPm.Tenant);
            service.Update(currententityPm);

            TableLastUpdateClass.UpdateTableHistory(currententityPm.Tenant, "GlobalZone");
        }

        public void DeleteGlobalZone(GlobalZonePM entityPm)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }
            globalZoneRepository = new GlobalZoneRepository(objectContext);
            GlobalZone removedEntity = globalZoneRepository.GetSingleGlobalZone(entityPm.Id, entityPm.Tenant);
            this.globalZoneRepository.Remove(removedEntity);
        }
    }
}