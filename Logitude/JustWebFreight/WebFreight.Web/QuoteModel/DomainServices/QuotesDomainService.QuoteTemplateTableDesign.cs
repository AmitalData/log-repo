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


        public IQueryable<QuoteTemplateTableDesign> GetQuoteTemplateTableDesigns(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateTableDesignRepository quoteTemplateTableDesignRepository;
            quoteTemplateTableDesignRepository = new QuoteTemplateTableDesignRepository(tenant);
            return quoteTemplateTableDesignRepository.GetQuoteTemplateTableDesigns(0);
        }

        public IQueryable<QuoteTemplateTableDesignPM> GetQuoteTemplateTableDesignsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateTableDesignQuery quoteTemplateTableDesignQuery = new QuoteTemplateTableDesignQuery(tenant);
            return quoteTemplateTableDesignQuery.GetQuoteTemplateTableDesignPMsByTenant(tenant);
        }



        public QuoteTemplateTableDesignPM GetQuoteTemplateTableDesignById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateTableDesignQuery quoteTemplateTableDesignQuery = new QuoteTemplateTableDesignQuery(tenant);
            QuoteTemplateTableDesignPM quoteTemplateTableDesign = quoteTemplateTableDesignQuery.GetSinglePM(id, tenant);
            return quoteTemplateTableDesign;
        }

        public QuoteTemplateTableDesignList GetSingleQuoteTemplateTableDesignList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateTableDesignRepository quoteTemplateTableDesignRepository;
            quoteTemplateTableDesignRepository = new QuoteTemplateTableDesignRepository(tenant);
            QuoteTemplateTableDesignQuery quoteTemplateTableDesignQuery = new QuoteTemplateTableDesignQuery(quoteTemplateTableDesignRepository);
            QuoteTemplateTableDesignList quoteTemplateTableDesignList = null;
            QuoteTemplateTableDesign quoteTemplateTableDesign = quoteTemplateTableDesignRepository.GetSingleQuoteTemplateTableDesign(id, tenant, false);

            if (quoteTemplateTableDesign != null)
            {
                List<QuoteTemplateTableDesign> singleEntityList = new List<QuoteTemplateTableDesign>();
                singleEntityList.Add(quoteTemplateTableDesign);

                IQueryable<QuoteTemplateTableDesign> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteTemplateTableDesignList> iQueryableEntityList = quoteTemplateTableDesignQuery.GetIQueryableEntityList(iQueryable);
                quoteTemplateTableDesignList = iQueryableEntityList.FirstOrDefault();
            }
            return quoteTemplateTableDesignList;
        }

        public IQueryable<QuoteTemplateTableDesignList> GetQuoteTemplateTableDesignList(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateTableDesignRepository quoteTemplateTableDesignRepository;
            quoteTemplateTableDesignRepository = new QuoteTemplateTableDesignRepository(tenant);
            QuoteTemplateTableDesignQuery quoteTemplateTableDesignQuery = new QuoteTemplateTableDesignQuery(quoteTemplateTableDesignRepository);
            IQueryable<QuoteTemplateTableDesign> quoteTemplateTableDesigns = quoteTemplateTableDesignRepository.GetQuoteTemplateTableDesigns(tenant);
            IQueryable<QuoteTemplateTableDesignList> query2 = quoteTemplateTableDesignQuery.GetIQueryableEntityList(quoteTemplateTableDesigns);
            return query2;
        }

        public IQueryable<QuoteTemplateTableDesignList> GetQuoteTemplateTableDesignFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateTableDesignRepository quoteTemplateTableDesignRepository;
            quoteTemplateTableDesignRepository = new QuoteTemplateTableDesignRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateTableDesign> quoteTemplateTableDesigns = quoteTemplateTableDesignRepository.GetQuoteTemplateTableDesigns(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            quoteTemplateTableDesigns = filter.GetFilteredQuery<QuoteTemplateTableDesign>(nonListQueryOperation, quoteTemplateTableDesigns);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            QuoteTemplateTableDesignQuery quoteTemplateTableDesignQuery = new QuoteTemplateTableDesignQuery(quoteTemplateTableDesignRepository);
            IQueryable<QuoteTemplateTableDesignList> query2 = quoteTemplateTableDesignQuery.GetIQueryableEntityList(quoteTemplateTableDesigns);

            query2 = filter.GetFilteredQuery<QuoteTemplateTableDesignList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteTemplateTableDesignList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> QuoteTemplateTableDesignObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QuoteTemplateTableDesign", tenant).ToList();

                ObjectField objectField = (from a in QuoteTemplateTableDesignObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateTableDesignList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateTableDesignList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateTableDesignList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateTableDesignList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateTableDesignList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.BorderColor);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.BorderColor);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetQuoteTemplateTableDesignFiltersCount(byte[] xmlFilters, int tenant)
        {
            QuoteTemplateTableDesignRepository quoteTemplateTableDesignRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            quoteTemplateTableDesignRepository = new QuoteTemplateTableDesignRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateTableDesign> quoteTemplateTableDesigns = quoteTemplateTableDesignRepository.GetQuoteTemplateTableDesigns(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            quoteTemplateTableDesigns = filter.GetFilteredQuery<QuoteTemplateTableDesign>(nonListQueryOperation, quoteTemplateTableDesigns);
            QuoteTemplateTableDesignQuery quoteTemplateTableDesignQuery = new QuoteTemplateTableDesignQuery(quoteTemplateTableDesignRepository);
            IQueryable<QuoteTemplateTableDesignList> query2 = quoteTemplateTableDesignQuery.GetIQueryableEntityList(quoteTemplateTableDesigns);

            query2 = filter.GetFilteredQuery<QuoteTemplateTableDesignList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertQuoteTemplateTableDesign(QuoteTemplateTableDesignPM currentquoteTemplateTableDesign)
        {
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateTableDesign.Tenant);
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateTableDesign.Tenant);
            }
            QuoteTemplateTableDesignService service = new QuoteTemplateTableDesignService(objectContext, currentquoteTemplateTableDesign.Tenant);
            service.Create(currentquoteTemplateTableDesign);

            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateTableDesign.Tenant, "QuoteTemplateTableDesign");



        }

        public void UpdateQuoteTemplateTableDesign(QuoteTemplateTableDesignPM currentquoteTemplateTableDesign)
        {
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateTableDesign.Tenant);

            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateTableDesign.Tenant);
            }

            string entityName = "QuoteTemplateTableDesign" + currentquoteTemplateTableDesign.Id + currentquoteTemplateTableDesign.Tenant;
            string entityPmName = "QuoteTemplateTableDesignPM" + currentquoteTemplateTableDesign.Id + currentquoteTemplateTableDesign.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            QuoteTemplateTableDesignService service = new QuoteTemplateTableDesignService(objectContext, currentquoteTemplateTableDesign.Tenant);
            service.Update(currentquoteTemplateTableDesign);
            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateTableDesign.Tenant, "QuoteTemplateTableDesign");

        }

        public void DeleteQuoteTemplateTableDesign(QuoteTemplateTableDesignPM quoteTemplateTableDesign)
        {
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(quoteTemplateTableDesign.Tenant);
            }
            QuoteTemplateTableDesignRepository quoteTemplateTableDesignRepository;
            quoteTemplateTableDesignRepository = new QuoteTemplateTableDesignRepository(objectContext);
            //DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            QuoteTemplateTableDesign entity = quoteTemplateTableDesignRepository.GetSingleQuoteTemplateTableDesign(quoteTemplateTableDesign.Id, quoteTemplateTableDesign.Tenant, false);
            quoteTemplateTableDesignRepository.Remove(entity);
        }

    }
}