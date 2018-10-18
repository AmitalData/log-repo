using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
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
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.QuoteModel.DomainServices
{
    public partial class QuotesDomainService
    {
        private QuoteStageQuery myQuoteStageQuery;
        private QuoteStageRepository myQuoteStageRepository;

        public QuoteStagePM GetSingleQuoteStagePM(string entityId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("QuoteStage", "READ", tenant);

            myQuoteStageQuery = new QuoteStageQuery(tenant);
            QuoteStagePM myResult = myQuoteStageQuery.GetSinglePM(entityId, tenant);            

            return myResult;
        }

        public QuoteStageList GetSingleQuoteStageList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("QuoteStage", "READ", tenant);

            myQuoteStageRepository = new QuoteStageRepository(tenant);
            myQuoteStageQuery = new QuoteStageQuery(myQuoteStageRepository);

            QuoteStageList entityList = null;
            QuoteStage entityPOCO = myQuoteStageRepository.GetSingleQuoteStage(id,tenant);

            if (entityPOCO != null)
            {
                List<QuoteStage> singleEntityList = new List<QuoteStage>();
                singleEntityList.Add(entityPOCO);

                IQueryable<QuoteStage> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteStageList> iQueryableEntityList = myQuoteStageQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public void UpdateQuoteStageList(QuoteStageList entityList)
        {

        }

        public List<QuoteStageList> GetQuoteStageLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("QuoteStage", "READ", tenant);

            myQuoteStageRepository = new QuoteStageRepository(tenant);
            myQuoteStageQuery = new QuoteStageQuery(myQuoteStageRepository);

            IQueryable<QuoteStage> iQueryable = myQuoteStageRepository.GetQuoteStages(tenant);
            IQueryable<QuoteStageList> query2 = myQuoteStageQuery.GetIQueryableEntityList(iQueryable);

            List<QuoteStageList> myResult = query2.ToList();
            return myResult;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<QuoteStageList> GetQuoteStageFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("QuoteStage", "READ", tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            myQuoteStageRepository = new QuoteStageRepository(tenant);
            myQuoteStageQuery = new QuoteStageQuery(myQuoteStageRepository);

            IQueryable<QuoteStage> iQueryable = myQuoteStageRepository.GetQuoteStages(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<QuoteStage>(nonListQueryOperation, iQueryable);
            int skippedEntities = queryOperations.PageIndex;

            IQueryable<QuoteStageList> query2 = myQuoteStageQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<QuoteStageList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteStageList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> objectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QuoteStage", tenant).ToList();

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
                                query2 = sortClass.GetSorterQuery<QuoteStageList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteStageList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteStageList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteStageList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteStageList, bool>(queryOperations, query2);
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
                query2 = query2.OrderBy(d => d.Name);
            }

            query2 = query2.Skip(skippedEntities);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetQuoteStageFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("QuoteStage", "READ", tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            myQuoteStageRepository = new QuoteStageRepository(tenant);
            myQuoteStageQuery = new QuoteStageQuery(myQuoteStageRepository);

            IQueryable<QuoteStage> iQueryable = myQuoteStageRepository.GetQuoteStages(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            iQueryable = filter.GetFilteredQuery<QuoteStage>(nonListQueryOperation, iQueryable);

            IQueryable<QuoteStageList> query2 = myQuoteStageQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<QuoteStageList>(listQueryOperation, query2);
            int count = query2.Count();

            return count;
        }

        public void InsertQuoteStage(QuoteStagePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("QuoteStage", "NEW", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(entityPM.Tenant);
            }

            QuoteStageService service = new QuoteStageService(objectContext, entityPM.Tenant);
            service.Create(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "QuoteStage");
        }

        public void UpdateQuoteStage(QuoteStagePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
            SecurityUtility.CheckContactFeature("QuoteStage", "UPDATE", entityPM.Tenant);

            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(entityPM.Tenant);
            }

            QuoteStageService service = new QuoteStageService(objectContext, entityPM.Tenant);
            service.Update(entityPM);

            TableLastUpdateClass.UpdateTableHistory(entityPM.Tenant, "QuoteStage");
        }
    }
}