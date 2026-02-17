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
using WebFreight.Web.WebServices;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.QuoteModel.DomainServices
{
    public partial class QuotesDomainService
    {
        public IQueryable<QuoteTemplateSection> GetQuoteTemplateSections(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateSectionRepository quoteTemplateSectionRepository;
            quoteTemplateSectionRepository = new QuoteTemplateSectionRepository(tenant);
            return quoteTemplateSectionRepository.GetQuoteTemplateSections(0);
        }

        public IQueryable<QuoteTemplateSectionPM> GetQuoteTemplateSectionsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateSectionQuery quoteTemplateSectionQuery = new QuoteTemplateSectionQuery(tenant);
            return quoteTemplateSectionQuery.GetQuoteTemplateSectionPMsByTenant(tenant);
        }


        public List<QuoteTemplateSectionPM> GetQuoteTemplateSectionsByTemplateId(string templateId,int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateSectionQuery quoteTemplateSectionQuery = new QuoteTemplateSectionQuery(tenant);
            return quoteTemplateSectionQuery.GetQuoteTemplateSectionPMsByTemplateId(templateId,tenant);
        }

        public List<QuoteTemplateSectionPM> GetQuoteEditableTemplateSectionsByTemplateId(string templateId,string quoteId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            QuoteTemplateSectionQuery quoteTemplateSectionQuery = new QuoteTemplateSectionQuery(tenant);
            return quoteTemplateSectionQuery.GetQuoteEditableTemplateSectionPMsByTemplateId(templateId, quoteId, tenant);
        }

        public QuoteTemplateSectionPM GetQuoteTemplateSectionById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            //departmentRepository = new DepartmentRepository(tenant);
            QuoteTemplateSectionQuery quoteTemplateSectionQuery = new QuoteTemplateSectionQuery(tenant);
            QuoteTemplateSectionPM quoteTemplateSection = quoteTemplateSectionQuery.GetSinglePM(id, tenant);
            return quoteTemplateSection;
        }

        public QuoteTemplateSectionList GetSingleQuoteTemplateSectionList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateSectionRepository quoteTemplateSectionRepository;
            quoteTemplateSectionRepository = new QuoteTemplateSectionRepository(tenant);
            QuoteTemplateSectionQuery quoteTemplateSectionQuery = new QuoteTemplateSectionQuery(quoteTemplateSectionRepository);
            QuoteTemplateSectionList quoteTemplateSectionList = null;
            QuoteTemplateSection quoteTemplateSection = quoteTemplateSectionRepository.GetSingleQuoteTemplateSection(id, tenant, false);

            if (quoteTemplateSection != null)
            {
                List<QuoteTemplateSection> singleEntityList = new List<QuoteTemplateSection>();
                singleEntityList.Add(quoteTemplateSection);

                IQueryable<QuoteTemplateSection> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteTemplateSectionList> iQueryableEntityList = quoteTemplateSectionQuery.GetIQueryableEntityList(iQueryable);
                quoteTemplateSectionList = iQueryableEntityList.FirstOrDefault();
            }
            return quoteTemplateSectionList;
        }

        public IQueryable<QuoteTemplateSectionList> GetQuoteTemplateSectionList(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateSectionRepository quoteTemplateSectionRepository;
            quoteTemplateSectionRepository = new QuoteTemplateSectionRepository(tenant);
            QuoteTemplateSectionQuery quoteTemplateSectionQuery = new QuoteTemplateSectionQuery(quoteTemplateSectionRepository);
            IQueryable<QuoteTemplateSection> quoteTemplateSection = quoteTemplateSectionRepository.GetQuoteTemplateSections(tenant);
            IQueryable<QuoteTemplateSectionList> query2 = quoteTemplateSectionQuery.GetIQueryableEntityList(quoteTemplateSection);
            return query2;
        }

        public IQueryable<QuoteTemplateSectionList> GetQuoteTemplateSectionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
            QuoteTemplateSectionRepository quoteTemplateSectionRepository;
            quoteTemplateSectionRepository = new QuoteTemplateSectionRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateSection> quoteTemplateSections = quoteTemplateSectionRepository.GetQuoteTemplateSections(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            quoteTemplateSections = filter.GetFilteredQuery<QuoteTemplateSection>(nonListQueryOperation, quoteTemplateSections);
            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            QuoteTemplateSectionQuery quoteTemplateSectionQuery = new QuoteTemplateSectionQuery(quoteTemplateSectionRepository);
            IQueryable<QuoteTemplateSectionList> query2 = quoteTemplateSectionQuery.GetIQueryableEntityList(quoteTemplateSections);

            query2 = filter.GetFilteredQuery<QuoteTemplateSectionList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteTemplateSectionList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> QuoteTemplateSectionObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QuoteTemplateSection", tenant).ToList();

                ObjectField objectField = (from a in QuoteTemplateSectionObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateSectionList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateSectionList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateSectionList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateSectionList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTemplateSectionList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.SectionDocId);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.SectionDocId);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetQuoteTemplateSectionFiltersCount(byte[] xmlFilters, int tenant)
        {
            QuoteTemplateSectionRepository quoteTemplateSectionRepository;
            SecurityUtility.AuthenticationOnTenant(tenant);
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

            quoteTemplateSectionRepository = new QuoteTemplateSectionRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteTemplateSection> quoteTemplateSections = quoteTemplateSectionRepository.GetQuoteTemplateSections(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            quoteTemplateSections = filter.GetFilteredQuery<QuoteTemplateSection>(nonListQueryOperation, quoteTemplateSections);
            QuoteTemplateSectionQuery quoteTemplateSectionQuery = new QuoteTemplateSectionQuery(quoteTemplateSectionRepository);
            IQueryable<QuoteTemplateSectionList> query2 = quoteTemplateSectionQuery.GetIQueryableEntityList(quoteTemplateSections);

            query2 = filter.GetFilteredQuery<QuoteTemplateSectionList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertQuoteTemplateSection(QuoteTemplateSectionPM currentquoteTemplateSection)
        {
            QuoteTemplateWebService quoteTemplateWebService = new QuoteTemplateWebService();

            if (currentquoteTemplateSection.Name != "ader" && currentquoteTemplateSection.Name != "Foer" && currentquoteTemplateSection.QuoteTemplateSectionTypeCode != "PC" && currentquoteTemplateSection.QuoteTemplateSectionTypeCode != "PP")
            {
                if (currentquoteTemplateSection.Templatedata != null)
                {
                    currentquoteTemplateSection.SectionDocId = quoteTemplateWebService.UploadQuoteTemplateSectionDataFile(currentquoteTemplateSection.Templatedata, null, currentquoteTemplateSection.Tenant);
                }
            }

            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateSection.Tenant);
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateSection.Tenant);
            }

            QuoteTemplateSectionService service = new QuoteTemplateSectionService(objectContext, currentquoteTemplateSection.Tenant);
            service.Create(currentquoteTemplateSection);

            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateSection.Tenant, "QuoteTemplateSection");
             


        }

        public void UpdateQuoteTemplateSection(QuoteTemplateSectionPM currentquoteTemplateSection)
        {
           
            ////SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", currentquoteTemplateSection.Tenant);

            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(currentquoteTemplateSection.Tenant);
            }

            string entityName = "QuoteTemplateSection" + currentquoteTemplateSection.Id + currentquoteTemplateSection.Tenant;
            string entityPmName = "QuoteTemplateSectionPM" + currentquoteTemplateSection.Id + currentquoteTemplateSection.Tenant;
            if (CacheManager.CacheWrapper.Get(entityName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityName);
            }
            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
            {
                CacheManager.CacheWrapper.Invalidate(entityPmName);
            }

            QuoteTemplateSectionService service = new QuoteTemplateSectionService(objectContext, currentquoteTemplateSection.Tenant);
            service.Update(currentquoteTemplateSection);

            TableLastUpdateClass.UpdateTableHistory(currentquoteTemplateSection.Tenant, "QuoteTemplateSection");

          
        }

        public void DeleteQuoteTemplateSection(QuoteTemplateSectionPM quoteTemplateSection)
        {
            if (objectContext == null)
            {
                objectContext = QuotesContext.GetContext(quoteTemplateSection.Tenant);
            }
            QuoteTemplateSectionRepository quoteTemplateSectionRepository;
            quoteTemplateSectionRepository = new QuoteTemplateSectionRepository(objectContext);
            //DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
            QuoteTemplateSection entity = quoteTemplateSectionRepository.GetSingleQuoteTemplateSection(quoteTemplateSection.Id, quoteTemplateSection.Tenant, false);
            quoteTemplateSectionRepository.Remove(entity);
        }




        [Invoke]
        public bool ResettingQuoteTemplateSectionModification(string quoteId, string quotetemplatesectionId, int tenant)
        {


            QuoteTemplateSectionModificationRepository quoteTemplateSectionModificationRepository = new QuoteTemplateSectionModificationRepository(tenant);

            //
            QuoteTemplateSectionModification quoteTemplateSectionModification = quoteTemplateSectionModificationRepository.GetSingelQuoteTemplateSectionModification(quoteId, quotetemplatesectionId, tenant);

            quoteTemplateSectionModificationRepository.Remove(quoteTemplateSectionModification);

            quoteTemplateSectionModificationRepository.SubmitChanges();
            return true;

        }







    }
}