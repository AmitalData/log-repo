using Logitude.Server.Tools.Counters;
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

        public IQueryable<QuoteTemplateTextDesign> GetQuoteTemplateTextDesigns(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateTextDesignRepository quoteTemplateTextDesignRepository;
            quoteTemplateTextDesignRepository = new QuoteTemplateTextDesignRepository(tenant);
            return quoteTemplateTextDesignRepository.GetQuoteTemplateTextDesigns(0);
        }
      
        public IQueryable<QuoteTemplateTextDesignPM> GetQuoteTemplateTextDesignsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateTextDesignQuery quoteTemplateTextDesignQuery = new QuoteTemplateTextDesignQuery(tenant);
            return quoteTemplateTextDesignQuery.GetQuoteTemplateTextDesignPMsByTenant(tenant);
        }





        public QuoteTemplateTextDesignPM GetQuoteTemplateTextDesignById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateTextDesignQuery quoteTemplateTextDesignQuery = new QuoteTemplateTextDesignQuery(tenant);
            QuoteTemplateTextDesignPM quoteTemplateTextDesign = quoteTemplateTextDesignQuery.GetSinglePM(id, tenant);
            return quoteTemplateTextDesign;
        }



        public QuoteTemplateTextDesignList GetSingleQuoteTemplateTextDesignList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateTextDesignRepository quoteTemplateTextDesignRepository;
            quoteTemplateTextDesignRepository = new QuoteTemplateTextDesignRepository(tenant);
            QuoteTemplateTextDesignQuery quoteTemplateTextDesignQuery = new QuoteTemplateTextDesignQuery(quoteTemplateTextDesignRepository);
            QuoteTemplateTextDesignList quoteTemplateTextDesignList = null;
            QuoteTemplateTextDesign quoteTemplateTextDesign = quoteTemplateTextDesignRepository.GetSingleQuoteTemplateTextDesign(id, tenant, false );
               
            if (quoteTemplateTextDesign != null)
            {
                List<QuoteTemplateTextDesign> singleEntityList = new List<QuoteTemplateTextDesign>();
                singleEntityList.Add(quoteTemplateTextDesign);

                IQueryable<QuoteTemplateTextDesign> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteTemplateTextDesignList> iQueryableEntityList = quoteTemplateTextDesignQuery.GetIQueryableEntityList(iQueryable);
                quoteTemplateTextDesignList = iQueryableEntityList.FirstOrDefault();
            }
            return quoteTemplateTextDesignList;
        }

        public IQueryable<QuoteTemplateTextDesignList> GetQuoteTemplateTextDesignList(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateTextDesignRepository quoteTemplateTextDesignRepository;
            quoteTemplateTextDesignRepository = new QuoteTemplateTextDesignRepository(tenant);
            QuoteTemplateTextDesignQuery quoteTemplateTextDesignQuery = new QuoteTemplateTextDesignQuery(quoteTemplateTextDesignRepository);
            IQueryable<QuoteTemplateTextDesign> quoteTemplateTextDesigns = quoteTemplateTextDesignRepository.GetQuoteTemplateTextDesigns(tenant);
            IQueryable<QuoteTemplateTextDesignList> query2 = quoteTemplateTextDesignQuery.GetIQueryableEntityList(quoteTemplateTextDesigns);
            return query2;
        }

        public IQueryable<QuoteTemplateTextDesignList> GetQuoteTemplateTextDesignFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateTextDesignRepository quoteTemplateTextDesignRepository;
            quoteTemplateTextDesignRepository = new QuoteTemplateTextDesignRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateTextDesign> quoteTemplateTextDesigns = quoteTemplateTextDesignRepository.GetQuoteTemplateTextDesigns(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            quoteTemplateTextDesigns = filter.GetFilteredQuery<QuoteTemplateTextDesign>(nonListQueryOperation, quoteTemplateTextDesigns);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            QuoteTemplateTextDesignQuery quoteTemplateTextDesignQuery = new QuoteTemplateTextDesignQuery(quoteTemplateTextDesignRepository);
            IQueryable<QuoteTemplateTextDesignList> query2 = quoteTemplateTextDesignQuery.GetIQueryableEntityList(quoteTemplateTextDesigns);

            query2 = filter.GetFilteredQuery<QuoteTemplateTextDesignList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteTemplateTextDesignList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> QuoteTemplateTextDesignObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QuoteTemplateTextDesign", tenant).ToList();

                ObjectField objectField = (from a in QuoteTemplateTextDesignObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateTextDesignList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateTextDesignList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateTextDesignList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateTextDesignList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateTextDesignList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.FontFamily);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.FontFamily);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetQuoteTemplateTextDesignFiltersCount(byte[] xmlFilters, int tenant)
        {
            QuoteTemplateTextDesignRepository quoteTemplateTextDesignRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            quoteTemplateTextDesignRepository = new QuoteTemplateTextDesignRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateTextDesign> quoteTemplateTextDesigns = quoteTemplateTextDesignRepository.GetQuoteTemplateTextDesigns(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            quoteTemplateTextDesigns = filter.GetFilteredQuery<QuoteTemplateTextDesign>(nonListQueryOperation, quoteTemplateTextDesigns);
            QuoteTemplateTextDesignQuery quoteTemplateTextDesignQuery = new QuoteTemplateTextDesignQuery(quoteTemplateTextDesignRepository);
            IQueryable<QuoteTemplateTextDesignList> query2 = quoteTemplateTextDesignQuery.GetIQueryableEntityList(quoteTemplateTextDesigns);

            query2 = filter.GetFilteredQuery<QuoteTemplateTextDesignList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertQuoteTemplateTextDesign(QuoteTemplateTextDesignPM currentquoteTemplateTextDesign)
        {

           // currentquoteTemplateTextDesign.Id = IdCounter.GetNumber("QuoteTemplateTextDesign", 1).ToString();
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateTextDesign.Tenant);
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateTextDesign.Tenant);
            }
            QuoteTemplateTextDesignService service = new QuoteTemplateTextDesignService(objectContext, currentquoteTemplateTextDesign.Tenant);
            service.Create(currentquoteTemplateTextDesign);

            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateTextDesign.Tenant, "QuoteTemplateTextDesign");



        }

        public void UpdateQuoteTemplateTextDesign(QuoteTemplateTextDesignPM currentquoteTemplateTextDesign)
        {
            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateTextDesign.Tenant);

            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateTextDesign.Tenant);
            }

            string entityName = "QuoteTemplateTextDesign" + currentquoteTemplateTextDesign.Id + currentquoteTemplateTextDesign.Tenant;
            string entityPmName = "QuoteTemplateTextDesignPM" + currentquoteTemplateTextDesign.Id + currentquoteTemplateTextDesign.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            QuoteTemplateTextDesignService service = new QuoteTemplateTextDesignService(objectContext, currentquoteTemplateTextDesign.Tenant);
            service.Update(currentquoteTemplateTextDesign);
            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateTextDesign.Tenant, "QuoteTemplateTextDesign");

        }

        public void DeleteQuoteTemplateTextDesign(QuoteTemplateTextDesignPM quoteTemplateTextDesign)
        {
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(quoteTemplateTextDesign.Tenant);
            }
            QuoteTemplateTextDesignRepository quoteTemplateTextDesignRepository;
            quoteTemplateTextDesignRepository = new QuoteTemplateTextDesignRepository(objectContext);
            //DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            QuoteTemplateTextDesign entity = quoteTemplateTextDesignRepository.GetSingleQuoteTemplateTextDesign(quoteTemplateTextDesign.Id, quoteTemplateTextDesign.Tenant , false);
            quoteTemplateTextDesignRepository.Remove(entity);
        }

    }
}