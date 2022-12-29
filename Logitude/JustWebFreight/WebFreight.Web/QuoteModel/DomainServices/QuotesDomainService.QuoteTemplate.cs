using Logitude.Server.Tools.Counters;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
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
using WebFreight.Web.WebServices;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.Helpers;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.QuoteModel.CustomFilters;

namespace WebFreight.Web.QuoteModel.DomainServices
{
    public partial class QuotesDomainService
    {

        public IQueryable<QuoteTemplate> GetQuoteTemplates(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateRepository quoteTemplateRepository;
            quoteTemplateRepository = new QuoteTemplateRepository(tenant);
            return quoteTemplateRepository.GetQuoteTemplates(tenant);
        }

        public IQueryable<QuoteTemplatePM> GetQuoteTemplatesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(tenant);
            return quoteTemplateQuery.GetQuoteTemplatePMsByTenant(tenant);
        }

        public QuoteTemplatePM GetSinglePMByQuoteId(string id, string quoteId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(tenant);
            return quoteTemplateQuery.GetSinglePMByQuoteId(id, quoteId, tenant);
        }


        public IQueryable<QuoteTemplateList> GetQuoteTemplateListsByQuoteTemplateTypeAndTenant(string quotetemplatetype, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateRepository quoteTemplateRepository;
            quoteTemplateRepository = new QuoteTemplateRepository(tenant);
            QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(quoteTemplateRepository);
            IQueryable<QuoteTemplate> quoteTemplates = quoteTemplateRepository.GetQuoteTemplatesByType(quotetemplatetype, tenant);
            IQueryable<QuoteTemplateList> query2 = quoteTemplateQuery.GetIQueryableEntityList(quoteTemplates);
            return query2;
        }
        public QuoteTemplatePM GetQuoteTemplateById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(tenant);
            QuoteTemplatePM quoteTemplate = quoteTemplateQuery.GetSinglePM(id, tenant);
            return quoteTemplate;
        }

        public QuoteTemplateList GetSingleQuoteTemplateList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //  //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateRepository quoteTemplateRepository;
            quoteTemplateRepository = new QuoteTemplateRepository(tenant);
            QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(quoteTemplateRepository);
            QuoteTemplateList quoteTemplateList = null;
            QuoteTemplate quoteTemplate = quoteTemplateRepository.GetSingleQuoteTemplate(id, tenant);

            if (quoteTemplate != null)
            {
                List<QuoteTemplate> singleEntityList = new List<QuoteTemplate>();
                singleEntityList.Add(quoteTemplate);

                IQueryable<QuoteTemplate> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteTemplateList> iQueryableEntityList = quoteTemplateQuery.GetIQueryableEntityList(iQueryable);
                quoteTemplateList = iQueryableEntityList.FirstOrDefault();
            }
            return quoteTemplateList;
        }

        public IQueryable<QuoteTemplateList> GetQuoteTemplateList(int tenant ,string queryName ,  bool isCustomerCare)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            // //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateRepository quoteTemplateRepository;
            quoteTemplateRepository = new QuoteTemplateRepository(tenant);
            QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(quoteTemplateRepository);
            IQueryable<QuoteTemplate> quoteTemplates = null;


            if (queryName == "FromTenant")
            {

                if (isCustomerCare)
                {
                    quoteTemplates = quoteTemplateRepository.GetAllQuoteTemplates();
                }
                else
                {
                    quoteTemplates = quoteTemplateRepository.GetQuoteTemplates(tenant);
                }

            }
            else
            {
                quoteTemplates = quoteTemplateRepository.GetQuoteTemplates(tenant);

            }
           



            IQueryable<QuoteTemplateList> query2 = quoteTemplateQuery.GetIQueryableEntityList(quoteTemplates);
            return query2;
        }

        //public IQueryable<QuoteTemplateList> GetAllQuoteTemplateList(int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    // //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
        //    QuoteTemplateRepository quoteTemplateRepository;
        //    quoteTemplateRepository = new QuoteTemplateRepository(tenant);
        //    QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(quoteTemplateRepository);
        //    IQueryable<QuoteTemplate> quoteTemplates = quoteTemplateRepository.GetAllQuoteTemplates();
        //    IQueryable<QuoteTemplateList> query2 = quoteTemplateQuery.GetIQueryableEntityList(quoteTemplates);
        //    return query2;
        //}

        //public IQueryable<QuoteTemplateList> GetQuoteTemplateFilters(byte[] xmlFilters, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("QuoteTemplate", "READ", tenant);
        //    QuoteTemplateRepository quoteTemplateRepository;
        //    quoteTemplateRepository = new QuoteTemplateRepository(tenant);
        //    MemoryStream memorystream = new MemoryStream(xmlFilters);
        //    XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
        //    QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
        //    GenericFilter filter = new GenericFilter();
        //    GenericSort sortClass = new GenericSort();
        //    IQueryable<QuoteTemplate> quoteTemplates = quoteTemplateRepository.GetQuoteTemplates(tenant);

        //    QueryOperations nonListQueryOperation = new QueryOperations();
        //    nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
        //    QueryOperations listQueryOperation = new QueryOperations();
        //    listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

        //    quoteTemplates = filter.GetFilteredQuery<QuoteTemplate>(nonListQueryOperation, quoteTemplates);
        //    int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
        //    QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(quoteTemplateRepository);
        //    IQueryable<QuoteTemplateList> query2 = quoteTemplateQuery.GetIQueryableEntityList(quoteTemplates);

        //    query2 = filter.GetFilteredQuery<QuoteTemplateList>(listQueryOperation, query2);

        //    if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
        //    {
        //        PropertyInfo propInfo = typeof(QuoteTemplateList).GetProperty(queryOperations.SortByColumnName);
        //        List<ObjectField> QuoteTemplateObjectFields = ObjectFieldsRepository.GetObjectFieldsByObjectTableName("QuoteTemplate", tenant).ToList();

        //        ObjectField objectField = (from a in QuoteTemplateObjectFields
        //                                   where a.FieldName == queryOperations.SortByColumnName
        //                                   select a).FirstOrDefault();

        //        if (objectField != null)
        //        {
        //            switch (objectField.DataTypeCode.ToLower())
        //            {
        //                case "text":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<QuoteTemplateList, string>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "double":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<QuoteTemplateList, double>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "datetime":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<QuoteTemplateList, DateTime>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "integer":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<QuoteTemplateList, int>(queryOperations, query2);
        //                        break;
        //                    }
        //                case "boolean":
        //                    {
        //                        query2 = sortClass.GetSorterQuery<QuoteTemplateList, bool>(queryOperations, query2);
        //                        break;
        //                    }
        //                default:
        //                    {
        //                        query2 = query2.OrderByDescending(d => d.FooterDocId);
        //                        break;
        //                    }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        query2 = query2.OrderByDescending(d => d.FooterDocId);
        //    }

        //    query2 = query2.Skip(skippedPorts);
        //    query2 = query2.Take(queryOperations.PageSize);
        //    return query2;
        //}

        public int GetQuoteTemplateFiltersCount(byte[] xmlFilters, int tenant)
        {

            QuoteTemplateRepository quoteTemplateRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
            // //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            quoteTemplateRepository = new QuoteTemplateRepository(tenant);
            QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(quoteTemplateRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteTemplate> iQueryable = quoteTemplateRepository.GetQuoteTemplateByTenant(tenant, null);

            QuoteTemplateFilter customFilters = new QuoteTemplateFilter(tenant);
            iQueryable = customFilters.GetQuoteTemplateFilteredQuery(queryOperations, iQueryable);
            QueryOperations nonListQueryOperation = new QueryOperations();


            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<QuoteTemplate>(nonListQueryOperation, iQueryable);

            IQueryable<QuoteTemplateList> query2 = quoteTemplateQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<QuoteTemplateList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;

            //QuoteTemplateRepository quoteTemplateRepository;
            //SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("QuoteTemplate", "READ", tenant);

            //quoteTemplateRepository = new QuoteTemplateRepository(tenant);

            //MemoryStream memorystream = new MemoryStream(xmlFilters);
            //XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            //QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            //GenericFilter filter = new GenericFilter();
            //GenericSort sortClass = new GenericSort();
            //IQueryable<QuoteTemplate> quoteTemplates = quoteTemplateRepository.GetQuoteTemplates(tenant);

            //QueryOperations nonListQueryOperation = new QueryOperations();
            //nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            //QueryOperations listQueryOperation = new QueryOperations();
            //listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            //quoteTemplates = filter.GetFilteredQuery<QuoteTemplate>(nonListQueryOperation, quoteTemplates);
            //QuoteTemplateQuery quoteTemplateQuery = new QuoteTemplateQuery(quoteTemplateRepository);
            //IQueryable<QuoteTemplateList> query2 = quoteTemplateQuery.GetIQueryableEntityList(quoteTemplates);

            //query2 = filter.GetFilteredQuery<QuoteTemplateList>(listQueryOperation, query2);
            //int count = query2.Count();
            //return count;
        }

        public void InsertQuoteTemplate(QuoteTemplatePM currentQuoteTemplate)
        {
            // //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentQuoteTemplate.Tenant);
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentQuoteTemplate.Tenant);
            }
            QuoteTemplateService service = new QuoteTemplateService(objectContext, currentQuoteTemplate.Tenant);
            service.Create(currentQuoteTemplate);

            TableLastUpdateClass.UpdateTableHistory(currentQuoteTemplate.Tenant, "QuoteTemplate");



        }

        public void UpdateQuoteTemplate(QuoteTemplatePM currentQuoteTemplate)
        {
            // //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentQuoteTemplate.Tenant);
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentQuoteTemplate.Tenant);
            }

            string entityName = "QuoteTemplate" + currentQuoteTemplate.Id + currentQuoteTemplate.Tenant;
            string entityPmName = "QuoteTemplatePM" + currentQuoteTemplate.Id + currentQuoteTemplate.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }






            QuoteTemplateService service = new QuoteTemplateService(objectContext, currentQuoteTemplate.Tenant);
            service.Update(currentQuoteTemplate);
            TableLastUpdateClass.UpdateTableHistory(currentQuoteTemplate.Tenant, "QuoteTemplate");

        }


        public void UpdateQuoteTemplateList(QuoteTemplateList currentQuoteTemplate)
        {

        }

        public void DeleteQuoteTemplate(QuoteTemplatePM QuoteTemplate)
        {
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(QuoteTemplate.Tenant);
            }
            QuoteTemplateRepository quoteTemplateRepository;
            quoteTemplateRepository = new QuoteTemplateRepository(objectContext);
            //DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            QuoteTemplate entity = quoteTemplateRepository.GetSingleQuoteTemplate(QuoteTemplate.Id, QuoteTemplate.Tenant);
            quoteTemplateRepository.Remove(entity);
        }


        public IQueryable<QuoteTemplateList> GetQuoteTemplateFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateRepository quoteTemplateRepository = new QuoteTemplateRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            #region Restrictions region
            AddRestrictionFilters(queryOperations, "QuoteTemplate", tenant);
            #endregion


            QuoteTemplateFilter Quotefilters = new QuoteTemplateFilter(tenant);
            IQueryable<QuoteTemplate> QuoteTemplates = quoteTemplateRepository.GetQuoteTemplateByTenant(tenant, null);
            QuoteTemplates = Quotefilters.GetQuoteTemplateFilteredQuery(queryOperations, QuoteTemplates);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            QuoteTemplates = filter.GetFilteredQuery<QuoteTemplate>(nonListQueryOperation, QuoteTemplates);
            int numOfQuoteTemplates = QuoteTemplates.Count();
            int skippedQuoteTemplates = queryOperations.PageIndex;

            QuoteTemplateQuery quoteTemplataQuery = new QuoteTemplateQuery(tenant);
            IQueryable<QuoteTemplateList> query2 = quoteTemplataQuery.GetIQueryableEntityList(QuoteTemplates);
            query2 = filter.GetFilteredQuery<QuoteTemplateList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteTemplateList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> QuoteTemplateObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QuoteTemplate", tenant).ToList();

                ObjectField objectField = (from a in QuoteTemplateObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.CreateDate);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.CreateDate);
            }

            query2 = query2.Skip(skippedQuoteTemplates);
            query2 = query2.Take(queryOperations.PageSize);


            List<QuoteTemplateList> listQuery = query2.ToList();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver(tenant);
            customFieldResolver.SetCustomFieldsValues("QuoteTemplate", tenant, listQuery.Cast<object>().ToList());
            

            return listQuery.AsQueryable();
        }
     


        [Invoke]
        public QuoteTemplatePM CopyQuoteTemplate(QuoteTemplateList quoteTemplate, string copyName, string userid, int tenant)
        {
            QuoteTemplateHelper quoteTemplateHelper = new QuoteTemplateHelper();
            return quoteTemplateHelper.CopyQuoteTemplate(quoteTemplate.Id, copyName, userid, tenant);
        }

    }
}
