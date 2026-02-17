using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using System.ServiceModel.DomainServices.Server;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateVesselList(VesselList currentEntity)
        {
        }

        public IQueryable<Vessel> GetVessels(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Vessel", "READ", tenant);

            vesselRepository = new VesselRepository(tenant);
            return vesselRepository.GetVesselsByTenant(0);
        }

        public VesselPM GetSingleVessel(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Vessel", "READ", tenant);

            vesselQuery = new VesselQuery(tenant);
            return vesselQuery.GetSinglePM(id, tenant);
        }

        public VesselList GetSingleVesselList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Vessel", "READ", tenant);

            vesselRepository = new VesselRepository(tenant);
            VesselList vesselList = null;
            Vessel vessel = vesselRepository.GetSingleVessel(id, tenant);

            if (vessel != null)
            {
                List<Vessel> singleEntityList = new List<Vessel>();
                singleEntityList.Add(vessel);

                vesselQuery = new VesselQuery(vesselRepository);
                IQueryable<Vessel> iQueryable = singleEntityList.AsQueryable();
                IQueryable<VesselList> iQueryableEntityList = vesselQuery.GetIQueryableEntityList(iQueryable);
                vesselList = iQueryableEntityList.FirstOrDefault();
            }
            return vesselList;
        }

        public IQueryable<VesselList> GetVesselLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Vessel", "READ", tenant);

            vesselRepository = new VesselRepository(tenant);
            vesselQuery = new VesselQuery(vesselRepository);

            IQueryable<Vessel> iQueryable = vesselRepository.GetVesselsByTenant(tenant);
            IQueryable<VesselList> query2 = vesselQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<VesselList> GetVesselFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Vessel", "READ", tenant);

            vesselRepository = new VesselRepository(tenant);
            vesselQuery = new VesselQuery(vesselRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Vessel> iQueryable = vesselRepository.GetVesselsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Vessel>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<VesselList> query2 = vesselQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<VesselList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(VesselList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Vessel", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<VesselList, string>(queryOperations, query2);
                                break;
                            }

                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<VesselList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<VesselList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<VesselList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<VesselList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<VesselList, bool>(queryOperations, query2);
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

        public int GetVesselFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Vessel", "READ", tenant);

            vesselRepository = new VesselRepository(tenant);
            vesselQuery = new VesselQuery(vesselRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<Vessel> iQueryable = vesselRepository.GetVesselsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Vessel>(nonListQueryOperation, iQueryable);

            IQueryable<VesselList> query2 = vesselQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<VesselList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertVessel(VesselPM entityPM)
        {
            SecurityUtility.CheckContactFeature("Vessel", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            VesselService service = new VesselService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Vessel");
        }

        public void UpdateVessel(VesselPM entityPM)
        {
            SecurityUtility.CheckContactFeature("Vessel", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            VesselService service = new VesselService(objectContext, entityPM.Tenant);
            service.Update(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "Vessel");
        }

        public void DeleteVessel(VesselPM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }

            vesselRepository = new VesselRepository(objectContext);
            Vessel entityStatus = vesselRepository.GetSingleVessel(entity.Id, entity.Tenant);
            vesselRepository.Remove(entityStatus);
        }
    }
}