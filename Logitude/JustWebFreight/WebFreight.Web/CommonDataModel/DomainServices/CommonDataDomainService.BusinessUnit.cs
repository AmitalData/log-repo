using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        private BusinessUnitRepository businessUnitRepository;

        public void UpdateBusinessUnit(BusinessUnitList entityList)
        {

        }

        public BusinessUnitPM GetSingleBusinessUnitPM(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BusinessUnit", "READ", tenant);

            BusinessUnitQuery businessUnitQuery = new BusinessUnitQuery(tenant);
            return businessUnitQuery.GetSinglePM(id, tenant);
        }

        public BusinessUnitList GetSingleBusinessUnitList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BusinessUnit", "READ", tenant);

            businessUnitRepository = new BusinessUnitRepository(tenant);
            BusinessUnitQuery businessUnitQuery = new BusinessUnitQuery(businessUnitRepository);

            BusinessUnitList entityList = null;
            BusinessUnit entityPOCO = businessUnitRepository.GetSingleBusinessUnit(id, tenant);

            if (entityPOCO != null)
            {
                entityList = new BusinessUnitList()
                {
                    Id = entityPOCO.Id,
                    Tenant = entityPOCO.Tenant,
                    Name = entityPOCO.Name,
                    InActive = entityPOCO.InActive,
                    ParentId = entityPOCO.ParentId,
                    SearchFields = entityPOCO.SearchFields,
                };

                if (!string.IsNullOrEmpty(entityList.ParentId))
                {
                    BusinessUnit myParent = businessUnitRepository.GetSingleBusinessUnit(entityList.ParentId, tenant);
                    if (myParent != null)
                    {
                        entityList.ParentName = myParent.Name;
                    }
                }
            }

            return entityList;
        }

        public IQueryable<BusinessUnitList> GetBusinessUnitLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BusinessUnit", "READ", tenant);

            businessUnitRepository = new BusinessUnitRepository(tenant);
            BusinessUnitQuery businessUnitQuery = new BusinessUnitQuery(businessUnitRepository);

            IQueryable<BusinessUnit> businessUnits = businessUnitRepository.GetBusinessUnits(tenant);
            IQueryable<BusinessUnitList> query2 = businessUnitQuery.GetIQueryableEntityList(businessUnits);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<BusinessUnitList> GetBusinessUnitFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BusinessUnit", "READ", tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            businessUnitRepository = new BusinessUnitRepository(tenant);
            BusinessUnitQuery businessUnitQuery = new BusinessUnitQuery(businessUnitRepository);
            IQueryable<BusinessUnit> businessUnits = businessUnitRepository.GetBusinessUnits(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            businessUnits = filter.GetFilteredQuery<BusinessUnit>(nonListQueryOperation, businessUnits);
            int skippedEntities = queryOperations.PageIndex;

            IQueryable<BusinessUnitList> query2 = businessUnitQuery.GetIQueryableEntityList(businessUnits);

            query2 = filter.GetFilteredQuery<BusinessUnitList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(BusinessUnitList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> objectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("BusinessUnit", tenant).ToList();

                ObjectField objectField = (from a in objectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<BusinessUnitList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<BusinessUnitList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<BusinessUnitList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<BusinessUnitList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<BusinessUnitList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }

            query2 = query2.Skip(skippedEntities);
            query2 = query2.Take(queryOperations.PageSize);

            return query2;
        }

        public int GetBusinessUnitFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("BusinessUnit", "READ", tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            businessUnitRepository = new BusinessUnitRepository(tenant);
            BusinessUnitQuery businessUnitQuery = new BusinessUnitQuery(businessUnitRepository);
            IQueryable<BusinessUnit> businessUnits = businessUnitRepository.GetBusinessUnits(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            businessUnits = filter.GetFilteredQuery<BusinessUnit>(nonListQueryOperation, businessUnits);

            IQueryable<BusinessUnitList> query2 = businessUnitQuery.GetIQueryableEntityList(businessUnits);

            query2 = filter.GetFilteredQuery<BusinessUnitList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertBusinessUnit(BusinessUnitPM entityPM)
        {
            SecurityUtility.CheckContactFeature("BusinessUnit", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            BusinessUnitService service = new BusinessUnitService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "BusinessUnit");
        }

        public void UpdateBusinessUnit(BusinessUnitPM entityPM)
        {
            SecurityUtility.CheckContactFeature("BusinessUnit", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPM.Tenant);
            }

            BusinessUnitService service = new BusinessUnitService(objectContext, entityPM.Tenant);
            service.Update(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "BusinessUnit");
        }

        public void DeleteBusinessUnit(BusinessUnitPM entityPM)
        {

        }
    }
}