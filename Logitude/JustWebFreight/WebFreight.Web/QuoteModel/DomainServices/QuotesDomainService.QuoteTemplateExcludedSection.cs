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
using System.ServiceModel.DomainServices.Server;
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
        public IQueryable<QuoteTemplateExcludedSection> GetQuoteTemplateExcludedSections(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateExcludedSectionRepository quoteTemplateExcludedSectionRepository;
            quoteTemplateExcludedSectionRepository = new QuoteTemplateExcludedSectionRepository(tenant);
            return quoteTemplateExcludedSectionRepository.GetQuoteTemplateExcludedSections(0);
        }

        public IQueryable<QuoteTemplateExcludedSectionPM> GetQuoteTemplateExcludedSectionsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateExcludedSectionQuery quoteTemplateExcludedSectionQuery = new QuoteTemplateExcludedSectionQuery(tenant);
            return quoteTemplateExcludedSectionQuery.GetQuoteTemplateExcludedSectionPMsByTenant(tenant);
        }

        //public QuoteTemplateExcludedSectionPM GetSingleQuoteTemplateExcludedSection(string id, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //     ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

        //    //departmentRepository = new DepartmentRepository(tenant);
        //    QuoteTemplateExcludedSectionQuery quoteTemplateExcludedSectionQuery = new QuoteTemplateExcludedSectionQuery(tenant);
        //    return quoteTemplateExcludedSectionQuery.GetSinglePM(id, tenant);
        //}

        public QuoteTemplateExcludedSectionPM GetQuoteTemplateExcludedSectionById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateExcludedSectionQuery quoteTemplateExcludedSectionQuery = new QuoteTemplateExcludedSectionQuery(tenant);
            QuoteTemplateExcludedSectionPM quoteTemplateExcludedSection = quoteTemplateExcludedSectionQuery.GetSinglePM(id, tenant);
            return quoteTemplateExcludedSection;
        }

        public QuoteTemplateExcludedSectionList GetSingleQuoteTemplateExcludedSectionList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateExcludedSectionRepository quoteTemplateExcludedSectionRepository;
            quoteTemplateExcludedSectionRepository = new QuoteTemplateExcludedSectionRepository(tenant);
            QuoteTemplateExcludedSectionQuery quoteTemplateExcludedSectionQuery = new QuoteTemplateExcludedSectionQuery(quoteTemplateExcludedSectionRepository);
            QuoteTemplateExcludedSectionList quoteTemplateExcludedSectionList = null;
            QuoteTemplateExcludedSection quoteTemplateExcludedSection = quoteTemplateExcludedSectionRepository.GetSingleQuoteTemplateExcludedSection(id, tenant);

            if (quoteTemplateExcludedSection != null)
            {
                List<QuoteTemplateExcludedSection> singleEntityList = new List<QuoteTemplateExcludedSection>();
                singleEntityList.Add(quoteTemplateExcludedSection);

                IQueryable<QuoteTemplateExcludedSection> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteTemplateExcludedSectionList> iQueryableEntityList = quoteTemplateExcludedSectionQuery.GetIQueryableEntityList(iQueryable);
                quoteTemplateExcludedSectionList = iQueryableEntityList.FirstOrDefault();
            }
            return quoteTemplateExcludedSectionList;
        }

        public IQueryable<QuoteTemplateExcludedSectionList> GetQuoteTemplateExcludedSectionList(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateExcludedSectionRepository quoteTemplateExcludedSectionRepository;
            quoteTemplateExcludedSectionRepository = new QuoteTemplateExcludedSectionRepository(tenant);
            QuoteTemplateExcludedSectionQuery quoteTemplateExcludedSectionQuery = new QuoteTemplateExcludedSectionQuery(quoteTemplateExcludedSectionRepository);
            IQueryable<QuoteTemplateExcludedSection> quoteTemplateExcludedSections = quoteTemplateExcludedSectionRepository.GetQuoteTemplateExcludedSections(tenant);
            IQueryable<QuoteTemplateExcludedSectionList> query2 = quoteTemplateExcludedSectionQuery.GetIQueryableEntityList(quoteTemplateExcludedSections);
            return query2;
        }

        public IQueryable<QuoteTemplateExcludedSectionList> GetQuoteTemplateExcludedSectionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateExcludedSectionRepository quoteTemplateExcludedSectionRepository;
            quoteTemplateExcludedSectionRepository = new QuoteTemplateExcludedSectionRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateExcludedSection> quoteTemplateExcludedSections = quoteTemplateExcludedSectionRepository.GetQuoteTemplateExcludedSections(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            quoteTemplateExcludedSections = filter.GetFilteredQuery<QuoteTemplateExcludedSection>(nonListQueryOperation, quoteTemplateExcludedSections);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            QuoteTemplateExcludedSectionQuery quoteTemplateExcludedSectionQuery = new QuoteTemplateExcludedSectionQuery(quoteTemplateExcludedSectionRepository);
            IQueryable<QuoteTemplateExcludedSectionList> query2 = quoteTemplateExcludedSectionQuery.GetIQueryableEntityList(quoteTemplateExcludedSections);

            query2 = filter.GetFilteredQuery<QuoteTemplateExcludedSectionList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteTemplateExcludedSectionList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> QuoteTemplateExcludedSectionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QuoteTemplateExcludedSection", tenant).ToList();

                ObjectField objectField = (from a in QuoteTemplateExcludedSectionObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateExcludedSectionList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateExcludedSectionList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateExcludedSectionList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateExcludedSectionList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateExcludedSectionList, bool>(queryOperations, query2);
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

        public int GetQuoteTemplateExcludedSectionFiltersCount(byte[] xmlFilters, int tenant)
        {
            QuoteTemplateExcludedSectionRepository quoteTemplateExcludedSectionRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            quoteTemplateExcludedSectionRepository = new QuoteTemplateExcludedSectionRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateExcludedSection> quoteTemplateExcludedSections = quoteTemplateExcludedSectionRepository.GetQuoteTemplateExcludedSections(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            quoteTemplateExcludedSections = filter.GetFilteredQuery<QuoteTemplateExcludedSection>(nonListQueryOperation, quoteTemplateExcludedSections);
            QuoteTemplateExcludedSectionQuery quoteTemplateExcludedSectionQuery = new QuoteTemplateExcludedSectionQuery(quoteTemplateExcludedSectionRepository);
            IQueryable<QuoteTemplateExcludedSectionList> query2 = quoteTemplateExcludedSectionQuery.GetIQueryableEntityList(quoteTemplateExcludedSections);

            query2 = filter.GetFilteredQuery<QuoteTemplateExcludedSectionList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertQuoteTemplateExcludedSection(QuoteTemplateExcludedSectionPM currentquoteTemplateExcludedSection)
        {
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateExcludedSection.Tenant);
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateExcludedSection.Tenant);
            }
            QuoteTemplateExcludedSectionService service = new QuoteTemplateExcludedSectionService(objectContext, currentquoteTemplateExcludedSection.Tenant);
            service.Create(currentquoteTemplateExcludedSection);

            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateExcludedSection.Tenant, "QuoteTemplateExcludedSection");



        }

        public void UpdateQuoteTemplateExcludedSection(QuoteTemplateExcludedSectionPM currentquoteTemplateExcludedSection)
        {
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateExcludedSection.Tenant);

            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateExcludedSection.Tenant);
            }

            string entityName = "QuoteTemplateExcludedSection" + currentquoteTemplateExcludedSection.Id + currentquoteTemplateExcludedSection.Tenant;
            string entityPmName = "QuoteTemplateExcludedSectionPM" + currentquoteTemplateExcludedSection.Id + currentquoteTemplateExcludedSection.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            QuoteTemplateExcludedSectionService service = new QuoteTemplateExcludedSectionService(objectContext, currentquoteTemplateExcludedSection.Tenant);
            service.Update(currentquoteTemplateExcludedSection);
            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateExcludedSection.Tenant, "QuoteTemplateExcludedSection");

        }

        public void DeleteQuoteTemplateExcludedSection(QuoteTemplateExcludedSectionPM quoteTemplateExcludedSection)
        {
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(quoteTemplateExcludedSection.Tenant);
            }
            QuoteTemplateExcludedSectionRepository quoteTemplateExcludedSectionRepository;
            quoteTemplateExcludedSectionRepository = new QuoteTemplateExcludedSectionRepository(objectContext);
            //DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            QuoteTemplateExcludedSection entity = quoteTemplateExcludedSectionRepository.GetSingleQuoteTemplateExcludedSection(quoteTemplateExcludedSection.Id, quoteTemplateExcludedSection.Tenant);
            quoteTemplateExcludedSectionRepository.Remove(entity);
        }




        public IQueryable<QuoteTemplateExcludedSectionPM> GetQuoteTemplateExcludedSectionsByQuotetemplateId(int tenant, string quotetemplateId)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateExcludedSectionQuery quoteTemplateExcludedSectionQuery = new QuoteTemplateExcludedSectionQuery(tenant);
            return quoteTemplateExcludedSectionQuery.GetQuoteTemplateExcludedSectionPMsByQuotetemplateId(tenant, quotetemplateId);
        }



        [Invoke]
        public bool MakeQuoteTemplateSectionsIncluded(string quoteId, string quotetemplateId, string quotetemplatesectionId, int tenant)
        {


            QuoteTemplateExcludedSectionRepository quoteTemplateExcludedSectionRepository = new QuoteTemplateExcludedSectionRepository(tenant);


            QuoteTemplateExcludedSection quoteTemplateExcludedSection = quoteTemplateExcludedSectionRepository.GetSingelExcludedSection(quoteId, quotetemplateId, quotetemplatesectionId, tenant);
           
            quoteTemplateExcludedSectionRepository.Remove(quoteTemplateExcludedSection);
            quoteTemplateExcludedSectionRepository.SubmitChanges();
            return true;

        }



    }
}