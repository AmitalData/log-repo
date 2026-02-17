using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
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
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using System.Net.Http;
using Intuit.Ipp.Core.Configuration;
using System.Net;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
	public partial class CommonDataDomainService
	{
        private ComputingPartnerQuery computingPartnerQuery;
        private ComputingPartnerRepository computingPartnerRepository;

        public void UpdateComputingPartnerList(ComputingPartnerList currentEntity)
        {

        }

        public ComputingPartnerPM GetSingleComputingPartnerPM(string id, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                   SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.CheckContactFeature("ComputingPartner", "READ", authToken.Tenant);

            computingPartnerQuery = new ComputingPartnerQuery(authToken.Tenant);
            ComputingPartnerPM entityPM = computingPartnerQuery.GetSinglePM(id, authToken.Tenant);

            return entityPM;
            }

            catch (Exception ex)
            {
                return null;
            }
         
        }

        public ComputingPartnerList GetSingleComputingPartnerList(string id, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("ComputingPartner", "READ", authToken.Tenant);
                computingPartnerRepository = new ComputingPartnerRepository(authToken.Tenant);
                ComputingPartnerList entityList = null;
                ComputingPartner entityPOCO = computingPartnerRepository.GetSingleComputingPartner(id, authToken.Tenant);

                if (entityPOCO != null)
                {
                    entityList = new ComputingPartnerList()
                    {
                        Id = entityPOCO.Id,
                        CreateDate = entityPOCO.CreateDate,
                        UpdateDate = entityPOCO.UpdateDate,
                        CreatedByUserId = entityPOCO.CreatedByUserId,
                        UpdatedByUserId = entityPOCO.UpdatedByUserId,
                        Name = entityPOCO.Name,
                        Remarks = entityPOCO.Remarks,
                        SearchFields = entityPOCO.SearchFields,
                        CreatedByUserName = entityPOCO.CreatedByUser == null ? "" : (entityPOCO.CreatedByUser.Contact == null ? "" : entityPOCO.CreatedByUser.Contact.EnglishName),
                        UpdatedByUserName = entityPOCO.UpdatedByUser == null ? "" : (entityPOCO.UpdatedByUser.Contact == null ? "" : entityPOCO.UpdatedByUser.Contact.EnglishName),
                        LoggedTenantId = tenant,
                        Code = entityPOCO.Code,
                        Tenant = entityPOCO.Tenant
                    };
                }

                return entityList;
            }
            catch(Exception e)
            {
                return null;
            }
          
        }

        public IQueryable<ComputingPartnerList> GetComputingPartnerLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ComputingPartner", "READ", tenant);

            computingPartnerRepository = new ComputingPartnerRepository(tenant);
            computingPartnerQuery = new ComputingPartnerQuery(computingPartnerRepository);

            IQueryable<ComputingPartner> iQueryable1 = computingPartnerRepository.GetComputingPartners(tenant);
            IQueryable<ComputingPartnerList> iQueryable2 = computingPartnerQuery.GetIQueryableEntityList(iQueryable1, tenant);

            return iQueryable2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ComputingPartnerList> GetComputingPartnerFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ComputingPartner", "READ", tenant);

            computingPartnerRepository = new ComputingPartnerRepository(tenant);
            computingPartnerQuery = new ComputingPartnerQuery(computingPartnerRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();        
            IQueryable<ComputingPartner> iQueryableData = computingPartnerRepository.GetComputingPartners(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryableData = filter.GetFilteredQuery<ComputingPartner>(nonListQueryOperation, iQueryableData);

            int skippedPorts = queryOperations.PageIndex;
            IQueryable<ComputingPartnerList> query2 = computingPartnerQuery.GetIQueryableEntityList(iQueryableData, tenant);
            query2 = filter.GetFilteredQuery<ComputingPartnerList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CountryCityList).GetProperty(queryOperations.SortByColumnName);

                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ComputingPartner", tenant).ToList();
                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ComputingPartnerList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ComputingPartnerList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ComputingPartnerList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ComputingPartnerList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ComputingPartnerList, bool>(queryOperations, query2);
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

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetComputingPartnerFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ComputingPartner", "READ", tenant);

            computingPartnerRepository = new ComputingPartnerRepository(tenant);
            computingPartnerQuery = new ComputingPartnerQuery(computingPartnerRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
                  IQueryable<ComputingPartner> iQueryableData = computingPartnerRepository.GetComputingPartners(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryableData = filter.GetFilteredQuery<ComputingPartner>(nonListQueryOperation, iQueryableData);

            IQueryable<ComputingPartnerList> query2 = computingPartnerQuery.GetIQueryableEntityList(iQueryableData, tenant);

            query2 = filter.GetFilteredQuery<ComputingPartnerList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertComputingPartner(ComputingPartnerPM entityPM)
        {
            int tenant = 0;

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ComputingPartner", "NEW", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            ComputingPartnerService service = new ComputingPartnerService(objectContext, entityPM);
            service.Create();

            TableLastUpdateClass.UpdateTableHistory(0, "ComputingPartner");
        }

        public void UpdateComputingPartner(ComputingPartnerPM entityPM)
        {
            int tenant = entityPM.LoggedTenantId;

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ComputingPartner", "UPDATE", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            #region Tables
            List<ComputingPartnerTablePM> myTablesChangeSet = ChangeSet.GetAssociatedChanges(entityPM, d => d.PartnerTables).Cast<ComputingPartnerTablePM>().ToList();
            foreach (ComputingPartnerTablePM itemPM in myTablesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Update: { itemPM.ChangeSetOp = ChangeSetOperation.Update; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }
                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }
            #endregion

            ComputingPartnerService service = new ComputingPartnerService(objectContext, entityPM);
            service.Update(myTablesChangeSet);

            TableLastUpdateClass.UpdateTableHistory(0, "ComputingPartner");
        }

        public void DeleteComputingPartner(ComputingPartnerPM entityPM)
        {

        }
	}
}