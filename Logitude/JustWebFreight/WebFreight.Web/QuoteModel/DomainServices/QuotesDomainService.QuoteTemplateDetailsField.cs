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
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.QuoteModel.DomainServices
{
    public partial class QuotesDomainService
    {
        public IQueryable<QuoteTemplateDetailsField> GetQuoteTemplateDetailsFields(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateDetailsFieldRepository quoteTemplateDetailsFieldRepository;
            quoteTemplateDetailsFieldRepository = new QuoteTemplateDetailsFieldRepository(tenant);
            return quoteTemplateDetailsFieldRepository.GetQuoteTemplateDetailsFields(0);
        }

        public IQueryable<QuoteTemplateDetailsFieldPM> GetQuoteTemplateDetailsFieldsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateDetailsFieldQuery quoteTemplateDetailsFieldQuery = new QuoteTemplateDetailsFieldQuery(tenant);
            return quoteTemplateDetailsFieldQuery.GetQuoteTemplateDetailsFieldPMsByTenant(tenant);
        }

        //public QuoteTemplateDetailsFieldPM GetSingleQuoteTemplateDetailsField(string id, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //     ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

        //    //departmentRepository = new DepartmentRepository(tenant);
        //    QuoteTemplateDetailsFieldQuery quoteTemplateDetailsFieldQuery = new QuoteTemplateDetailsFieldQuery(tenant);
        //    return quoteTemplateDetailsFieldQuery.GetSinglePM(id, tenant);
        //}

        public QuoteTemplateDetailsFieldPM GetQuoteTemplateDetailsFieldById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateDetailsFieldQuery quoteTemplateDetailsFieldQuery = new QuoteTemplateDetailsFieldQuery(tenant);
            QuoteTemplateDetailsFieldPM quoteTemplateDetailsField = quoteTemplateDetailsFieldQuery.GetSinglePM(id, tenant);
            return quoteTemplateDetailsField;
        }

        public QuoteTemplateDetailsFieldList GetSingleQuoteTemplateDetailsFieldList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateDetailsFieldRepository quoteTemplateDetailsFieldRepository;
            quoteTemplateDetailsFieldRepository = new QuoteTemplateDetailsFieldRepository(tenant);
            QuoteTemplateDetailsFieldQuery quoteTemplateDetailsFieldQuery = new QuoteTemplateDetailsFieldQuery(quoteTemplateDetailsFieldRepository);
            QuoteTemplateDetailsFieldList quoteTemplateDetailsFieldList = null;
            QuoteTemplateDetailsField quoteTemplateDetailsField = quoteTemplateDetailsFieldRepository.GetSingleQuoteTemplateDetailsField(id, tenant);

            if (quoteTemplateDetailsField != null)
            {
                List<QuoteTemplateDetailsField> singleEntityList = new List<QuoteTemplateDetailsField>();
                singleEntityList.Add(quoteTemplateDetailsField);

                IQueryable<QuoteTemplateDetailsField> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteTemplateDetailsFieldList> iQueryableEntityList = quoteTemplateDetailsFieldQuery.GetIQueryableEntityList(iQueryable);
                quoteTemplateDetailsFieldList = iQueryableEntityList.FirstOrDefault();
            }
            return quoteTemplateDetailsFieldList;
        }

        public IQueryable<QuoteTemplateDetailsFieldList> GetQuoteTemplateDetailsFieldList(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateDetailsFieldRepository quoteTemplateDetailsFieldRepository;
            quoteTemplateDetailsFieldRepository = new QuoteTemplateDetailsFieldRepository(tenant);
            QuoteTemplateDetailsFieldQuery quoteTemplateDetailsFieldQuery = new QuoteTemplateDetailsFieldQuery(quoteTemplateDetailsFieldRepository);
            IQueryable<QuoteTemplateDetailsField> quoteTemplateDetailsFields = quoteTemplateDetailsFieldRepository.GetQuoteTemplateDetailsFields(tenant);
            IQueryable<QuoteTemplateDetailsFieldList> query2 = quoteTemplateDetailsFieldQuery.GetIQueryableEntityList(quoteTemplateDetailsFields);
            return query2;
        }

        public IQueryable<QuoteTemplateDetailsFieldList> GetQuoteTemplateDetailsFieldFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateDetailsFieldRepository quoteTemplateDetailsFieldRepository;
            quoteTemplateDetailsFieldRepository = new QuoteTemplateDetailsFieldRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateDetailsField> quoteTemplateDetailsFields = quoteTemplateDetailsFieldRepository.GetQuoteTemplateDetailsFields(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            quoteTemplateDetailsFields = filter.GetFilteredQuery<QuoteTemplateDetailsField>(nonListQueryOperation, quoteTemplateDetailsFields);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            QuoteTemplateDetailsFieldQuery quoteTemplateDetailsFieldQuery = new QuoteTemplateDetailsFieldQuery(quoteTemplateDetailsFieldRepository);
            IQueryable<QuoteTemplateDetailsFieldList> query2 = quoteTemplateDetailsFieldQuery.GetIQueryableEntityList(quoteTemplateDetailsFields);

            query2 = filter.GetFilteredQuery<QuoteTemplateDetailsFieldList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteTemplateDetailsFieldList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> QuoteTemplateDetailsFieldObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QuoteTemplateDetailsField", tenant).ToList();

                ObjectField objectField = (from a in QuoteTemplateDetailsFieldObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateDetailsFieldList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateDetailsFieldList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateDetailsFieldList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateDetailsFieldList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateDetailsFieldList, bool>(queryOperations, query2);
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

        public int GetQuoteTemplateDetailsFieldFiltersCount(byte[] xmlFilters, int tenant)
        {
            QuoteTemplateDetailsFieldRepository quoteTemplateDetailsFieldRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
             ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            quoteTemplateDetailsFieldRepository = new QuoteTemplateDetailsFieldRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateDetailsField> quoteTemplateDetailsFields = quoteTemplateDetailsFieldRepository.GetQuoteTemplateDetailsFields(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            quoteTemplateDetailsFields = filter.GetFilteredQuery<QuoteTemplateDetailsField>(nonListQueryOperation, quoteTemplateDetailsFields);
            QuoteTemplateDetailsFieldQuery quoteTemplateDetailsFieldQuery = new QuoteTemplateDetailsFieldQuery(quoteTemplateDetailsFieldRepository);
            IQueryable<QuoteTemplateDetailsFieldList> query2 = quoteTemplateDetailsFieldQuery.GetIQueryableEntityList(quoteTemplateDetailsFields);

            query2 = filter.GetFilteredQuery<QuoteTemplateDetailsFieldList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertQuoteTemplateDetailsField(QuoteTemplateDetailsFieldPM currentquoteTemplateDetailsField)
        {
             ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateDetailsField.Tenant);
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateDetailsField.Tenant);
            }
            QuoteTemplateDetailsFieldService service = new QuoteTemplateDetailsFieldService(objectContext, currentquoteTemplateDetailsField.Tenant);
            service.Create(currentquoteTemplateDetailsField);

            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateDetailsField.Tenant, "QuoteTemplateDetailsField");



        }

        public void UpdateQuoteTemplateDetailsField(QuoteTemplateDetailsFieldPM currentquoteTemplateDetailsField)
        {
             ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateDetailsField.Tenant);

            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateDetailsField.Tenant);
            }

            string entityName = "QuoteTemplateDetailsField" + currentquoteTemplateDetailsField.Id + currentquoteTemplateDetailsField.Tenant;
            string entityPmName = "QuoteTemplateDetailsFieldPM" + currentquoteTemplateDetailsField.Id + currentquoteTemplateDetailsField.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            QuoteTemplateDetailsFieldService service = new QuoteTemplateDetailsFieldService(objectContext, currentquoteTemplateDetailsField.Tenant);
            service.Update(currentquoteTemplateDetailsField);
            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateDetailsField.Tenant, "QuoteTemplateDetailsField");

        }

        public void DeleteQuoteTemplateDetailsField(QuoteTemplateDetailsFieldPM quoteTemplateDetailsField)
        {
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(quoteTemplateDetailsField.Tenant);
            }
            QuoteTemplateDetailsFieldRepository quoteTemplateDetailsFieldRepository;
            quoteTemplateDetailsFieldRepository = new QuoteTemplateDetailsFieldRepository(objectContext);
            //DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            QuoteTemplateDetailsField entity = quoteTemplateDetailsFieldRepository.GetSingleQuoteTemplateDetailsField(quoteTemplateDetailsField.Id, quoteTemplateDetailsField.Tenant);
            quoteTemplateDetailsFieldRepository.Remove(entity);
        }




        public IQueryable<QuoteTemplateDetailsFieldPM> GetQuoteTemplateDetailsFieldsByQuotetemplateId(int tenant, string quotetemplateId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
             ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateDetailsFieldQuery quoteTemplateDetailsFieldQuery = new QuoteTemplateDetailsFieldQuery(tenant);
            return quoteTemplateDetailsFieldQuery.GetQuoteTemplateDetailsFieldPMsByQuotetemplateId(tenant, quotetemplateId);
        }

    }
}