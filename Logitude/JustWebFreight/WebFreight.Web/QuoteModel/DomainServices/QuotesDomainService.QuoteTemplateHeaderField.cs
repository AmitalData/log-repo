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
        public IQueryable<QuoteTemplateHeaderField> GetQuoteTemplateHeaderFields(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateHeaderFieldRepository quoteTemplateHeaderFieldRepository;
            quoteTemplateHeaderFieldRepository = new QuoteTemplateHeaderFieldRepository(tenant);
            return quoteTemplateHeaderFieldRepository.GetQuoteTemplateHeaderFields(0);
        }

        public IQueryable<QuoteTemplateHeaderFieldPM> GetQuoteTemplateHeaderFieldsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateHeaderFieldQuery quoteTemplateHeaderFieldQuery = new QuoteTemplateHeaderFieldQuery(tenant);
            return quoteTemplateHeaderFieldQuery.GetQuoteTemplateHeaderFieldPMsByTenant(tenant);
        }

        //public QuoteTemplateHeaderFieldPM GetSingleQuoteTemplateHeaderField(string id, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

        //    //departmentRepository = new DepartmentRepository(tenant);
        //    QuoteTemplateHeaderFieldQuery quoteTemplateHeaderFieldQuery = new QuoteTemplateHeaderFieldQuery(tenant);
        //    return quoteTemplateHeaderFieldQuery.GetSinglePM(id, tenant);
        //}

        public QuoteTemplateHeaderFieldPM GetQuoteTemplateHeaderFieldById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateHeaderFieldQuery quoteTemplateHeaderFieldQuery = new QuoteTemplateHeaderFieldQuery(tenant);
            QuoteTemplateHeaderFieldPM quoteTemplateHeaderField = quoteTemplateHeaderFieldQuery.GetSinglePM(id, tenant);
            return quoteTemplateHeaderField;
        }

        public QuoteTemplateHeaderFieldList GetSingleQuoteTemplateHeaderFieldList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateHeaderFieldRepository quoteTemplateHeaderFieldRepository;
            quoteTemplateHeaderFieldRepository = new QuoteTemplateHeaderFieldRepository(tenant);
            QuoteTemplateHeaderFieldQuery quoteTemplateHeaderFieldQuery = new QuoteTemplateHeaderFieldQuery(quoteTemplateHeaderFieldRepository);
            QuoteTemplateHeaderFieldList quoteTemplateHeaderFieldList = null;
            QuoteTemplateHeaderField quoteTemplateHeaderField = quoteTemplateHeaderFieldRepository.GetSingleQuoteTemplateHeaderField(id, tenant);

            if (quoteTemplateHeaderField != null)
            {
                List<QuoteTemplateHeaderField> singleEntityList = new List<QuoteTemplateHeaderField>();
                singleEntityList.Add(quoteTemplateHeaderField);

                IQueryable<QuoteTemplateHeaderField> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteTemplateHeaderFieldList> iQueryableEntityList = quoteTemplateHeaderFieldQuery.GetIQueryableEntityList(iQueryable);
                quoteTemplateHeaderFieldList = iQueryableEntityList.FirstOrDefault();
            }
            return quoteTemplateHeaderFieldList;
        }

        public IQueryable<QuoteTemplateHeaderFieldList> GetQuoteTemplateHeaderFieldList(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateHeaderFieldRepository quoteTemplateHeaderFieldRepository;
            quoteTemplateHeaderFieldRepository = new QuoteTemplateHeaderFieldRepository(tenant);
            QuoteTemplateHeaderFieldQuery quoteTemplateHeaderFieldQuery = new QuoteTemplateHeaderFieldQuery(quoteTemplateHeaderFieldRepository);
            IQueryable<QuoteTemplateHeaderField> quoteTemplateHeaderFields = quoteTemplateHeaderFieldRepository.GetQuoteTemplateHeaderFields(tenant);
            IQueryable<QuoteTemplateHeaderFieldList> query2 = quoteTemplateHeaderFieldQuery.GetIQueryableEntityList(quoteTemplateHeaderFields);
            return query2;
        }

        public IQueryable<QuoteTemplateHeaderFieldList> GetQuoteTemplateHeaderFieldFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateHeaderFieldRepository quoteTemplateHeaderFieldRepository;
            quoteTemplateHeaderFieldRepository = new QuoteTemplateHeaderFieldRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateHeaderField> quoteTemplateHeaderFields = quoteTemplateHeaderFieldRepository.GetQuoteTemplateHeaderFields(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            quoteTemplateHeaderFields = filter.GetFilteredQuery<QuoteTemplateHeaderField>(nonListQueryOperation, quoteTemplateHeaderFields);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            QuoteTemplateHeaderFieldQuery quoteTemplateHeaderFieldQuery = new QuoteTemplateHeaderFieldQuery(quoteTemplateHeaderFieldRepository);
            IQueryable<QuoteTemplateHeaderFieldList> query2 = quoteTemplateHeaderFieldQuery.GetIQueryableEntityList(quoteTemplateHeaderFields);

            query2 = filter.GetFilteredQuery<QuoteTemplateHeaderFieldList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteTemplateHeaderFieldList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> QuoteTemplateHeaderFieldObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QuoteTemplateHeaderField", tenant).ToList();

                ObjectField objectField = (from a in QuoteTemplateHeaderFieldObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateHeaderFieldList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateHeaderFieldList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateHeaderFieldList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateHeaderFieldList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateHeaderFieldList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.FieldCode);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.FieldCode);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetQuoteTemplateHeaderFieldFiltersCount(byte[] xmlFilters, int tenant)
        {
            QuoteTemplateHeaderFieldRepository quoteTemplateHeaderFieldRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            quoteTemplateHeaderFieldRepository = new QuoteTemplateHeaderFieldRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateHeaderField> quoteTemplateHeaderFields = quoteTemplateHeaderFieldRepository.GetQuoteTemplateHeaderFields(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            quoteTemplateHeaderFields = filter.GetFilteredQuery<QuoteTemplateHeaderField>(nonListQueryOperation, quoteTemplateHeaderFields);
            QuoteTemplateHeaderFieldQuery quoteTemplateHeaderFieldQuery = new QuoteTemplateHeaderFieldQuery(quoteTemplateHeaderFieldRepository);
            IQueryable<QuoteTemplateHeaderFieldList> query2 = quoteTemplateHeaderFieldQuery.GetIQueryableEntityList(quoteTemplateHeaderFields);

            query2 = filter.GetFilteredQuery<QuoteTemplateHeaderFieldList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertQuoteTemplateHeaderField(QuoteTemplateHeaderFieldPM currentquoteTemplateHeaderField)
        {
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateHeaderField.Tenant);
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateHeaderField.Tenant);
            }
            QuoteTemplateHeaderFieldService service = new QuoteTemplateHeaderFieldService(objectContext, currentquoteTemplateHeaderField.Tenant);
            service.Create(currentquoteTemplateHeaderField);
           
            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateHeaderField.Tenant, "QuoteTemplateHeaderField");



        }

        public void UpdateQuoteTemplateHeaderField(QuoteTemplateHeaderFieldPM currentquoteTemplateHeaderField)
        {
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateHeaderField.Tenant);

            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateHeaderField.Tenant);
            }

            string entityName = "QuoteTemplateHeaderField" + currentquoteTemplateHeaderField.Id + currentquoteTemplateHeaderField.Tenant;
            string entityPmName = "QuoteTemplateHeaderFieldPM" + currentquoteTemplateHeaderField.Id + currentquoteTemplateHeaderField.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            QuoteTemplateHeaderFieldService service = new QuoteTemplateHeaderFieldService(objectContext, currentquoteTemplateHeaderField.Tenant);
            service.Update(currentquoteTemplateHeaderField);
            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateHeaderField.Tenant, "QuoteTemplateHeaderField");

        }

        public void DeleteQuoteTemplateHeaderField(QuoteTemplateHeaderFieldPM quoteTemplateHeaderField)
        {
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(quoteTemplateHeaderField.Tenant);
            }
            QuoteTemplateHeaderFieldRepository quoteTemplateHeaderFieldRepository;
            quoteTemplateHeaderFieldRepository = new QuoteTemplateHeaderFieldRepository(objectContext);
            //DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            QuoteTemplateHeaderField entity = quoteTemplateHeaderFieldRepository.GetSingleQuoteTemplateHeaderField(quoteTemplateHeaderField.Id, quoteTemplateHeaderField.Tenant);
            quoteTemplateHeaderFieldRepository.Remove(entity);
        }



        public IQueryable<QuoteTemplateHeaderFieldPM> GetQuoteTemplateHeaderFieldsByQuotetemplateId(int tenant, string quotetemplateId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateHeaderFieldQuery quoteTemplateHeaderFieldQuery = new QuoteTemplateHeaderFieldQuery(tenant);
            return quoteTemplateHeaderFieldQuery.GetQuoteTemplateHeaderFieldPMsByQuotetemplateId(tenant, quotetemplateId);
        }


      //  GetQuoteTemplateHeaderFieldPMsByQuotetemplateId

    }
}