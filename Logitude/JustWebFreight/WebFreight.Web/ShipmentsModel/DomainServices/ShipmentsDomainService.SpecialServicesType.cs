using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
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
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.Tools.EntityService;

namespace WebFreight.Web.ShipmentsModel.DomainServices
{
    public partial class ShipmentsDomainService
    {
        private SpecialServicesTypeRepository specialServicesTypeRepository;
        private SpecialServicesTypeQuery specialServicesTypeQuery;

        public IQueryable<SpecialServicesTypePM> GetSpecialServicesTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SpecialServicesType", "READ", tenant);
          
            specialServicesTypeQuery = new SpecialServicesTypeQuery(tenant);
            return specialServicesTypeQuery.GetSpecialServicesTypePMsByTenant(tenant);
        }

        public SpecialServicesTypePM GetSingleSpecialServicesTypePM(string Id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SpecialServicesType", "READ", tenant);

            specialServicesTypeQuery = new SpecialServicesTypeQuery(tenant);
            return specialServicesTypeQuery.GetSingleSpecialServicesTypePM(Id, tenant);
        }
    
        public SpecialServicesTypeList GetSingleSpecialServicesTypeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SpecialServicesType", "READ", tenant);

            specialServicesTypeRepository = new SpecialServicesTypeRepository(tenant);
            specialServicesTypeQuery = new SpecialServicesTypeQuery(specialServicesTypeRepository);
            SpecialServicesTypeList SpecialServicesTypeList = null;
            SpecialServicesType SpecialServicesType = specialServicesTypeRepository.GetSingleSpecialServicesType(id, tenant);

            if (SpecialServicesType != null)
            {
                List<SpecialServicesType> singleEntityList = new List<SpecialServicesType>();
                singleEntityList.Add(SpecialServicesType);

                IQueryable<SpecialServicesType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<SpecialServicesTypeList> iQueryableEntityList = specialServicesTypeQuery.GetIQueryableEntityList(iQueryable);
                SpecialServicesTypeList = iQueryableEntityList.FirstOrDefault();
            }
            return SpecialServicesTypeList;
        }

        public IQueryable<SpecialServicesTypeList> GetSpecialServicesTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("SpecialServicesType", "READ", tenant);


            specialServicesTypeRepository = new SpecialServicesTypeRepository(tenant);
            specialServicesTypeQuery = new SpecialServicesTypeQuery(specialServicesTypeRepository);
            IQueryable<SpecialServicesType> specialServicesTypes = specialServicesTypeRepository.GetSpecialServicesTypes(tenant);
            IQueryable<SpecialServicesTypeList> query2 = specialServicesTypeQuery.GetIQueryableEntityList(specialServicesTypes).AsQueryable();
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<SpecialServicesTypeList> GetSpecialServicesTypeFilters(byte[] xmlFilters, int tenant)
        {
            specialServicesTypeRepository = new SpecialServicesTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SpecialServicesType> iQueryable = specialServicesTypeRepository.GetSpecialServicesTypes(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SpecialServicesType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            var query2 = from entity in iQueryable
                         select new SpecialServicesTypeList()
                         {
                             Code = entity.Code,
                             Id = entity.Id,
                             EnglishName = entity.EnglishName,
                             LocalName = entity.LocalName,
                             Tenant = entity.Tenant,
                             SearchFields = entity.SearchFields,
                             InActive = entity.InActive,
                         };

            query2 = filter.GetFilteredQuery<SpecialServicesTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(SpecialServicesTypeList).GetProperty(queryOperations.SortByColumnName);
                //ObjectFieldsRepository objectFieledsRepository = new ObjectFieldsRepository(tenant);
                List<ObjectField> secialServicesTypetObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("SpecialServicesType", tenant).ToList();

                ObjectField objectField = (from a in secialServicesTypetObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<SpecialServicesTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<SpecialServicesTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<SpecialServicesTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<SpecialServicesTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<SpecialServicesTypeList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.EnglishName);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.EnglishName);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetSpecialServicesTypeCount(byte[] xmlFilters, int tenant)
        {
            specialServicesTypeRepository = new SpecialServicesTypeRepository(tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SpecialServicesType> iQueryable = specialServicesTypeRepository.GetSpecialServicesTypes(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SpecialServicesType>(nonListQueryOperation, iQueryable);

            var query2 = from entity in iQueryable
                         select new SpecialServicesTypeList()
                         {
                             Code = entity.Code,
                             Id = entity.Id,
                             EnglishName = entity.EnglishName,
                             LocalName = entity.LocalName,
                             Tenant = entity.Tenant,
                             SearchFields = entity.SearchFields,
                             InActive = entity.InActive,
                         };

            query2 = filter.GetFilteredQuery<SpecialServicesTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertAccount(SpecialServicesTypePM entityPM)
        {

            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("SpecialServicesType", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = ShipmentsContext.GetContext(entityPM.Tenant);
            }

            SpecialServicesTypeService service = new SpecialServicesTypeService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            

        }

        public void UpdateAccount(SpecialServicesTypePM currentEntityPM)
        {
            SecurityUtility.AuthenticationOnTenant(currentEntityPM.Tenant);
            SecurityUtility.CheckContactFeature("SpecialServicesType", "UPDATE", currentEntityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = ShipmentsContext.GetContext(currentEntityPM.Tenant);
            }
            SpecialServicesTypeService service = new SpecialServicesTypeService(objectContext, currentEntityPM.Tenant);
            service.Update(currentEntityPM);

        }

       

    }
}