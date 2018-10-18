using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.CustomFilters;
using Logitude.BL.GlobalModel.EntityLists;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.Tools.DataMapping;
using Logitude.BL.GlobalModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.Security;

namespace WebFreight.Web.GlobalModel
{
    public partial class GlobalDomainService
    {
        private ApiCredintialsRepository ApiCredintialsRepository;
        private ApiCredintialsQuery ApiCredintialsQuery;

        //[RequiresAuthentication]
        //[Query(IsDefault = true)]
        //public IQueryable<ApiCredintials> GetTenants()
        //{
        //    SecurityUtility.CheckContactFeature("ApiCredintials", "READ", 0);
        //    return ApiCredintialsRepository.GetTenants();
        //}

        public ApiCredintialsPM GetSingleApiCredintialsPM(int tenant, string id)
        {
            ApiCredintialsRepository = new ApiCredintialsRepository(objectContext);
            ApiCredintialsQuery = new ApiCredintialsQuery();
            return ApiCredintialsQuery.GetSinglePM(id, tenant);
        }

        public void InsertApiCredintialstPM(ApiCredintialsPM entityPM)
        {
            var CommonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactsRepositorypository = new ContactRepository(CommonContext);
            ContactQuery contactQuery = new ContactQuery(contactsRepositorypository);
            ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant, true);
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                ApiCredintialsRepository = new ApiCredintialsRepository(objectContext);
                ApiCredintials entity = new ApiCredintials();
                entityPM.Id = IdCounter.GetNumber("ApiCredintials", entityPM.Tenant);
                entityPM.CreateDate = DateTime.Now;
                entityPM.CreatedBy = contact != null ? contact.EnglishName : "";
                entityPM.UpdateDate = DateTime.Now;
                entityPM.Tenant = entityPM.Tenant;
                entityPM.UpdatedBy = contact != null ? contact.EnglishName : "";
                entityPM.Tenant = entityPM.Tenant;
                ApiCredintialsMapping.MapEntity(entityPM, entity, true);
                ApiCredintialsRepository.Add(entity);
                ApiCredintialsRepository.SubmitChanges();
                scope.Complete();
            }
        }

        public void UpdateApiCredintialsPM(ApiCredintialsPM entityPM)
        {
            var CommonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactsRepositorypository = new ContactRepository(CommonContext);
            ContactQuery contactQuery = new ContactQuery(contactsRepositorypository);
            ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant, true);
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                entityPM.UpdateDate = DateTime.Now;
                entityPM.UpdatedBy = contact != null ? contact.LocalName : "";
                ApiCredintialsRepository = new ApiCredintialsRepository(objectContext);
                ApiCredintials entity = ApiCredintialsRepository.GetSingleApiCredintials(entityPM.Id, entityPM.Tenant);
                ApiCredintialsMapping.MapEntity(entityPM, entity, false);
                ApiCredintialsRepository.Update(entity);
                ApiCredintialsRepository.SubmitChanges();
                scope.Complete();
            }
            
        }

         
        public void DeleteApiCredintialsPM(ApiCredintialsPM tenant)
        {
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ApiCredintialsList> GetApiCredintialsFilters(byte[] xmlFilters, int tenant)
        {
            ApiCredintialsRepository = new ApiCredintialsRepository(objectContext);
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ApiCredintials", "READ", tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ApiCredintials> iQueryable = ApiCredintialsRepository.GetApiCredintials(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ApiCredintials>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            var query2 = from a in iQueryable
                         select new ApiCredintialsList()
                         {
                             Id = a.Id,
                             AllowedIPs = a.AllowedIPs,
                             CreateDate = a.CreateDate,
                             CreatedBy = a.CreatedBy,
                             HashedPrimaryAccessKey = a.HashedPrimaryAccessKey,
                             HashedSeconderyAccessKey = a.HashedSeconderyAccessKey,
                             maskedPrimaryAccessKey = a.maskedPrimaryAccessKey,
                             maskedSeconderyAccessKey = a.maskedSeconderyAccessKey,
                             UpdateDate = a.UpdateDate,
                             UpdatedBy = a.UpdatedBy,
                             UsedFor = a.UsedFor
                         };

            query2 = filter.GetFilteredQuery<ApiCredintialsList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ApiCredintialsList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ApiCredintials", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<ApiCredintialsList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ApiCredintialsList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ApiCredintialsList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ApiCredintialsList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ApiCredintialsList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Id);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetApiCredintialsFiltersCount(byte[] xmlFilters, int tenant)
        {
            ApiCredintialsRepository = new ApiCredintialsRepository(objectContext);
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ApiCredintials", "READ", tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ApiCredintials> iQueryable = ApiCredintialsRepository.GetApiCredintials(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ApiCredintials>(nonListQueryOperation, iQueryable);

            var query2 = from a in iQueryable
                         select new ApiCredintialsList()
                         {
                             Id = a.Id,
                             AllowedIPs = a.AllowedIPs,
                             CreateDate = a.CreateDate,
                             CreatedBy = a.CreatedBy,
                             HashedPrimaryAccessKey = a.HashedPrimaryAccessKey,
                             HashedSeconderyAccessKey = a.HashedSeconderyAccessKey,
                             maskedPrimaryAccessKey = a.maskedPrimaryAccessKey,
                             maskedSeconderyAccessKey = a.maskedSeconderyAccessKey,
                             UpdateDate = a.UpdateDate,
                             UpdatedBy = a.UpdatedBy,
                             UsedFor = a.UsedFor
                         };

            query2 = filter.GetFilteredQuery<ApiCredintialsList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        

        public IQueryable<ApiCredintialsPM> GetApiCredintialsPMs(int tenant)
        {
            ApiCredintialsRepository = new ApiCredintialsRepository(objectContext);
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ApiCredintials", "READ", tenant);
            ApiCredintialsQuery = new ApiCredintialsQuery(tenant);
            return ApiCredintialsQuery.GetApiCredintialsPMs(tenant);
        }

        public IQueryable<ApiCredintialsList> GetApiCredintialsLists(int tenant)
        {
            ApiCredintialsRepository = new ApiCredintialsRepository(objectContext);
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ApiCredintials", "READ", tenant);
            ApiCredintialsQuery = new ApiCredintialsQuery(tenant);
            return ApiCredintialsQuery.GetApiCredintialsLists(tenant);
        }

        public ApiCredintialsList GetSingleApiCredintialsList(string id, int tenant)
        {
            ApiCredintialsRepository = new ApiCredintialsRepository(objectContext);
            ApiCredintialsQuery = new ApiCredintialsQuery(ApiCredintialsRepository);
            return ApiCredintialsQuery.GetSingleList(tenant, id);
        }

        public void UpdateApiCredintialstList(ApiCredintialsList currentEntity)
        {

        }


    }
}