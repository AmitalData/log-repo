using Simplog.Data.CommonDataModel;
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
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.QuoteModel.DomainServices
{
    public partial class QuotesDomainService
    {

        public IQueryable<QuoteTemplateSetting> GetQuoteTemplateSettings(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateSettingRepository quoteTemplateSettingRepository;
            quoteTemplateSettingRepository = new QuoteTemplateSettingRepository(tenant);
            return quoteTemplateSettingRepository.GetQuoteTemplateSettings(0);
        }

        public IQueryable<QuoteTemplateSettingPM> GetQuoteTemplateSettingsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateSettingQuery quoteTemplateSettingQuery = new QuoteTemplateSettingQuery(tenant);
            return quoteTemplateSettingQuery.GetQuoteTemplateSettingPMsByTenant(tenant);
        }



        public QuoteTemplateSettingPM GetQuoteTemplateSettingById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateSettingQuery quoteTemplateSettingQuery = new QuoteTemplateSettingQuery(tenant);
            QuoteTemplateSettingPM quoteTemplateSetting = quoteTemplateSettingQuery.GetSinglePM(id, tenant);
            return quoteTemplateSetting;
        }

        public QuoteTemplateSettingList GetSingleQuoteTemplateSettingList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateSettingRepository quoteTemplateSettingRepository;
            quoteTemplateSettingRepository = new QuoteTemplateSettingRepository(tenant);
            QuoteTemplateSettingQuery quoteTemplateSettingQuery = new QuoteTemplateSettingQuery(quoteTemplateSettingRepository);
            QuoteTemplateSettingList quoteTemplateSettingList = null;
            QuoteTemplateSetting quoteTemplateSetting = quoteTemplateSettingRepository.GetSingleQuoteTemplateSetting(id, tenant);

            if (quoteTemplateSetting != null)
            {
                List<QuoteTemplateSetting> singleEntityList = new List<QuoteTemplateSetting>();
                singleEntityList.Add(quoteTemplateSetting);

                IQueryable<QuoteTemplateSetting> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteTemplateSettingList> iQueryableEntityList = quoteTemplateSettingQuery.GetIQueryableEntityList(iQueryable);
                quoteTemplateSettingList = iQueryableEntityList.FirstOrDefault();
            }
            return quoteTemplateSettingList;
        }

        public IQueryable<QuoteTemplateSettingList> GetQuoteTemplateSettingList(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateSettingRepository quoteTemplateSettingRepository;
            quoteTemplateSettingRepository = new QuoteTemplateSettingRepository(tenant);
            QuoteTemplateSettingQuery quoteTemplateSettingQuery = new QuoteTemplateSettingQuery(quoteTemplateSettingRepository);
            IQueryable<QuoteTemplateSetting> quoteTemplateSettings = quoteTemplateSettingRepository.GetQuoteTemplateSettings(tenant);
            IQueryable<QuoteTemplateSettingList> query2 = quoteTemplateSettingQuery.GetIQueryableEntityList(quoteTemplateSettings);
            return query2;
        }

        public IQueryable<QuoteTemplateSettingList> GetQuoteTemplateSettingFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateSettingRepository quoteTemplateSettingRepository;
            quoteTemplateSettingRepository = new QuoteTemplateSettingRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateSetting> quoteTemplateSettings = quoteTemplateSettingRepository.GetQuoteTemplateSettings(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            quoteTemplateSettings = filter.GetFilteredQuery<QuoteTemplateSetting>(nonListQueryOperation, quoteTemplateSettings);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            QuoteTemplateSettingQuery quoteTemplateSettingQuery = new QuoteTemplateSettingQuery(quoteTemplateSettingRepository);
            IQueryable<QuoteTemplateSettingList> query2 = quoteTemplateSettingQuery.GetIQueryableEntityList(quoteTemplateSettings);

            query2 = filter.GetFilteredQuery<QuoteTemplateSettingList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteTemplateSettingList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> QuoteTemplateSettingObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QuoteTemplateSetting", tenant).ToList();

                ObjectField objectField = (from a in QuoteTemplateSettingObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateSettingList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateSettingList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateSettingList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateSettingList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateSettingList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.ShowLocalLanguage);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.ShowLocalLanguage);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetQuoteTemplateSettingFiltersCount(byte[] xmlFilters, int tenant)
        {
            QuoteTemplateSettingRepository quoteTemplateSettingRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            quoteTemplateSettingRepository = new QuoteTemplateSettingRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateSetting> quoteTemplateSettings = quoteTemplateSettingRepository.GetQuoteTemplateSettings(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            quoteTemplateSettings = filter.GetFilteredQuery<QuoteTemplateSetting>(nonListQueryOperation, quoteTemplateSettings);
            QuoteTemplateSettingQuery quoteTemplateSettingQuery = new QuoteTemplateSettingQuery(quoteTemplateSettingRepository);
            IQueryable<QuoteTemplateSettingList> query2 = quoteTemplateSettingQuery.GetIQueryableEntityList(quoteTemplateSettings);

            query2 = filter.GetFilteredQuery<QuoteTemplateSettingList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertQuoteTemplateSetting(QuoteTemplateSettingPM currentquoteTemplateSetting)
        {
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateSetting.Tenant);
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateSetting.Tenant);
            }
            QuoteTemplateSettingService service = new QuoteTemplateSettingService(objectContext, currentquoteTemplateSetting.Tenant);
            service.Create(currentquoteTemplateSetting);

            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateSetting.Tenant, "QuoteTemplateSetting");



        }

        public void UpdateQuoteTemplateSetting(QuoteTemplateSettingPM currentquoteTemplateSetting)
        {
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateSetting.Tenant);

            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateSetting.Tenant);
            }

            string entityName = "QuoteTemplateSetting" + currentquoteTemplateSetting.Id + currentquoteTemplateSetting.Tenant;
            string entityPmName = "QuoteTemplateSettingPM" + currentquoteTemplateSetting.Id + currentquoteTemplateSetting.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            QuoteTemplateSettingService service = new QuoteTemplateSettingService(objectContext, currentquoteTemplateSetting.Tenant);
            service.Update(currentquoteTemplateSetting);
            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateSetting.Tenant, "QuoteTemplateSetting");

        }

        public void DeleteQuoteTemplateSetting(QuoteTemplateSettingPM quoteTemplateSetting)
        {
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(quoteTemplateSetting.Tenant);
            }
            QuoteTemplateSettingRepository quoteTemplateSettingRepository;
           quoteTemplateSettingRepository = new QuoteTemplateSettingRepository(objectContext);
            //DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
           QuoteTemplateSetting entity = quoteTemplateSettingRepository.GetSingleQuoteTemplateSetting(quoteTemplateSetting.Id, quoteTemplateSetting.Tenant);
           quoteTemplateSettingRepository.Remove(entity);
        }

    }
}