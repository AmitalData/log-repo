//using Simplog.Data.InfrastructureModel.EntityPOCOs;
//using Simplog.Data.InfrastructureModel.Repositories;
//using Simplog.Data.QuoteModel;
//using Simplog.Data.QuoteModel.EntityPOCOs;
//using Simplog.Data.QuoteModel.Repositories;
//using Simplog.Server.Infrastructure.DataContracts;
//using Simplog.Server.Infrastructure.Helpers;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Web;
//using System.Xml.Serialization;
//using WebFreight.Web.Helpers;
//using Logitude.BL.QuoteModel.EntityLists;
//using Logitude.BL.QuoteModel.EntityPMs;
//using Logitude.BL.QuoteModel.EntityQueries;
//using WebFreight.Web.QuoteModel.Tools.EntityService;
//using WebFreight.Web.Security;

//namespace WebFreight.Web.QuoteModel.DomainServices
//{
//    public partial class QuotesDomainService
//    {


//        public IQueryable<QuoteDocumentVersion> GetQuoteDocumentVersions(int tenant)
//        {
//            SecurityUtility.AuthenticationOnTenant(tenant);
//            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
//            QuoteDocumentVersionRepository quoteDocumentVersionRepository;
//            quoteDocumentVersionRepository = new QuoteDocumentVersionRepository(tenant);
//            return quoteDocumentVersionRepository.GetQuoteDocumentVersions(tenant);
//        }

//        public IQueryable<QuoteDocumentVersionPM> GetQuoteDocumentVersionsByTenant(int tenant)
//        {
//            SecurityUtility.AuthenticationOnTenant(tenant);
//            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

//            //departmentRepository = new DepartmentRepository(tenant);
//            QuoteDocumentVersionQuery quoteDocumentVersionQuery = new QuoteDocumentVersionQuery(tenant);
//            return quoteDocumentVersionQuery.GetQuoteDocumentVersionPMsByTenant(tenant);
//        }

//        public IQueryable<QuoteDocumentVersionPM> GetQuoteDocumentVersionsByQuoteId(string quoteId,int tenant)
//        {
//            SecurityUtility.AuthenticationOnTenant(tenant);
//            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
//            //departmentRepository = new DepartmentRepository(tenant);
//            QuoteDocumentVersionQuery quoteDocumentVersionQuery = new QuoteDocumentVersionQuery(tenant);
//            return quoteDocumentVersionQuery.GetQuoteDocumentVersionPMsByQuoteId(quoteId, tenant);
//        }


//        public QuoteDocumentVersionList GetSingleQuoteDocumentVersionList(string quoteid, int tenant ,int versionNumber)
//        {
//            SecurityUtility.AuthenticationOnTenant(tenant);
//            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
//            QuoteDocumentVersionRepository quoteDocumentVersionRepository;
//            quoteDocumentVersionRepository = new QuoteDocumentVersionRepository(tenant);
//            QuoteDocumentVersionQuery quoteQuoteDocumentVersionQuery = new QuoteDocumentVersionQuery(quoteDocumentVersionRepository);
//            QuoteDocumentVersionList quoteDocumentVersionList = null;
//            QuoteDocumentVersion quoteDocumentVersion = quoteDocumentVersionRepository.GetSingleQuoteDocumentVersion(quoteid, tenant, versionNumber);

//            if (quoteDocumentVersion != null)
//            {
//                List<QuoteDocumentVersion> singleEntityList = new List<QuoteDocumentVersion>();
//                singleEntityList.Add(quoteDocumentVersion);

//                IQueryable<QuoteDocumentVersion> iQueryable = singleEntityList.AsQueryable();
//                IQueryable<QuoteDocumentVersionList> iQueryableEntityList = quoteQuoteDocumentVersionQuery.GetIQueryableEntityList(iQueryable);
//                quoteDocumentVersionList = iQueryableEntityList.FirstOrDefault();
//            }
//            return quoteDocumentVersionList;
//        }



//        public IQueryable<QuoteDocumentVersionList> GetQuoteDocumentVersionList(int tenant)
//        {
//            SecurityUtility.AuthenticationOnTenant(tenant);
//            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
//            QuoteDocumentVersionRepository quoteDocumentVersionRepository;
//            quoteDocumentVersionRepository = new QuoteDocumentVersionRepository(tenant);
//            QuoteDocumentVersionQuery quoteDocumentVersionQuery = new QuoteDocumentVersionQuery(quoteDocumentVersionRepository);
//            IQueryable<QuoteDocumentVersion> quoteDocumentVersions = quoteDocumentVersionRepository.GetQuoteDocumentVersions(tenant);
//            IQueryable<QuoteDocumentVersionList> query2 = quoteDocumentVersionQuery.GetIQueryableEntityList(quoteDocumentVersions);
//            return query2;
//        }

//        public IQueryable<QuoteDocumentVersionList> GetQuoteDocumentVersionFilters(byte[] xmlFilters, int tenant)
//        {
//            SecurityUtility.AuthenticationOnTenant(tenant);
//            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);
//            QuoteDocumentVersionRepository quoteDocumentVersionRepository;
//            quoteDocumentVersionRepository = new QuoteDocumentVersionRepository(tenant);
//            MemoryStream memorystream = new MemoryStream(xmlFilters);
//            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
//            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
//            GenericFilter filter = new GenericFilter();
//            GenericSort sortClass = new GenericSort();
//            IQueryable<QuoteDocumentVersion> quoteDocumentVersions = quoteDocumentVersionRepository.GetQuoteDocumentVersions(tenant);

//            QueryOperations nonListQueryOperation = new QueryOperations();
//            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
//            QueryOperations listQueryOperation = new QueryOperations();
//            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

//            quoteDocumentVersions = filter.GetFilteredQuery<QuoteDocumentVersion>(nonListQueryOperation, quoteDocumentVersions);
//            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
//            QuoteDocumentVersionQuery quoteDocumentVersionQuery = new QuoteDocumentVersionQuery(quoteDocumentVersionRepository);
//            IQueryable<QuoteDocumentVersionList> query2 = quoteDocumentVersionQuery.GetIQueryableEntityList(quoteDocumentVersions);

//            query2 = filter.GetFilteredQuery<QuoteDocumentVersionList>(listQueryOperation, query2);

//            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
//            {
//                PropertyInfo propInfo = typeof(QuoteDocumentVersionList).GetProperty(queryOperations.SortByColumnName);
//                List<ObjectField> QuoteDocumentVersionObjectFields = ObjectFieldsRepository.GetObjectFieldsByObjectTableName("QuoteDocumentVersion", tenant).ToList();

//                ObjectField objectField = (from a in QuoteDocumentVersionObjectFields
//                                           where a.FieldName == queryOperations.SortByColumnName
//                                           select a).FirstOrDefault();

//                if (objectField != null)
//                {
//                    switch (objectField.DataTypeCode.ToLower())
//                    {
//                        case "text":
//                            {
//                                query2 = sortClass.GetSorterQuery<QuoteDocumentVersionList, string>(queryOperations, query2);
//                                break;
//                            }
//                        case "double":
//                            {
//                                query2 = sortClass.GetSorterQuery<QuoteDocumentVersionList, double>(queryOperations, query2);
//                                break;
//                            }
//                        case "datetime":
//                            {
//                                query2 = sortClass.GetSorterQuery<QuoteDocumentVersionList, DateTime>(queryOperations, query2);
//                                break;
//                            }
//                        case "integer":
//                            {
//                                query2 = sortClass.GetSorterQuery<QuoteDocumentVersionList, int>(queryOperations, query2);
//                                break;
//                            }
//                        case "boolean":
//                            {
//                                query2 = sortClass.GetSorterQuery<QuoteDocumentVersionList, bool>(queryOperations, query2);
//                                break;
//                            }
//                        default:
//                            {
//                                query2 = query2.OrderByDescending(d => d.CreateDate);
//                                break;
//                            }
//                    }
//                }
//            }
//            else
//            {
//                query2 = query2.OrderByDescending(d => d.CreateDate);
//            }

//            query2 = query2.Skip(skippedPorts);
//            query2 = query2.Take(queryOperations.PageSize);
//            return query2;
//        }



//        public int GetQuoteDocumentVersionFiltersCount(byte[] xmlFilters, int tenant)
//        {
//            QuoteDocumentVersionRepository quoteDocumentVersionRepository;
//            SecurityUtility.AuthenticationOnTenant(tenant);
//            //SecurityUtility.CheckContactFeature("General", "QUOTETMPLATES", tenant);

//            quoteDocumentVersionRepository = new QuoteDocumentVersionRepository(tenant);

//            MemoryStream memorystream = new MemoryStream(xmlFilters);
//            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
//            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
//            GenericFilter filter = new GenericFilter();
//            GenericSort sortClass = new GenericSort();
//            IQueryable<QuoteDocumentVersion> quoteDocumentVersions = quoteDocumentVersionRepository.GetQuoteDocumentVersions(tenant);

//            QueryOperations nonListQueryOperation = new QueryOperations();
//            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
//            QueryOperations listQueryOperation = new QueryOperations();
//            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
//            quoteDocumentVersions = filter.GetFilteredQuery<QuoteDocumentVersion>(nonListQueryOperation, quoteDocumentVersions);
//            QuoteDocumentVersionQuery quoteDocumentVersionQuery = new QuoteDocumentVersionQuery(quoteDocumentVersionRepository);
//            IQueryable<QuoteDocumentVersionList> query2 = quoteDocumentVersionQuery.GetIQueryableEntityList(quoteDocumentVersions);

//            query2 = filter.GetFilteredQuery<QuoteDocumentVersionList>(listQueryOperation, query2);
//            int count = query2.Count();
//            return count;
//        }

//        public void InsertQuoteDocumentVersion(QuoteDocumentVersionPM currentquoteDocumentVersion)
//        {
//            //SecurityUtility.CheckContactFeature("QuoteDocumentVersion", "NEW", currentquoteDocumentVersion.Tenant);
//            if (objectContext == null)
//            {
//                objectContext = QuotesContext.GetContext(currentquoteDocumentVersion.Tenant);
//            }
//            QuoteDocumentVersionService service = new QuoteDocumentVersionService(objectContext, currentquoteDocumentVersion.Tenant);
//            service.Create(currentquoteDocumentVersion);

//            TableLastUpdateClass.UpdateTableHistory(currentquoteDocumentVersion.Tenant, "QuoteDocumentVersion");
//        }



//        public void UpdateQuoteDocumentVersion(QuoteDocumentVersionPM currentquoteDocumentVersion)
//        {
//            //SecurityUtility.CheckContactFeature("QuoteDocumentVersion", "UPDATE", currentquoteDocumentVersion.Tenant);

//            if (objectContext == null)
//            {
//                objectContext = QuotesContext.GetContext(currentquoteDocumentVersion.Tenant);
//            }

//            string entityName = "QuoteDocumentVersion" + currentquoteDocumentVersion.QuoteId + currentquoteDocumentVersion.Tenant + currentquoteDocumentVersion.VersionNumber;
//            string entityPmName = "QuoteDocumentVersionPM" + currentquoteDocumentVersion.QuoteId + currentquoteDocumentVersion.Tenant + currentquoteDocumentVersion.VersionNumber;
//            if (CacheManager.CacheWrapper.Get(entityName) != null)
//            {
//                CacheManager.CacheWrapper.Invalidate(entityName);
//            }
//            if (CacheManager.CacheWrapper.Get(entityPmName) != null)
//            {
//                CacheManager.CacheWrapper.Invalidate(entityPmName);
//            }

//            QuoteDocumentVersionService service = new QuoteDocumentVersionService(objectContext, currentquoteDocumentVersion.Tenant);
//            service.Update(currentquoteDocumentVersion);
//            TableLastUpdateClass.UpdateTableHistory(currentquoteDocumentVersion.Tenant, "QuoteDocumentVersion");

//        }

//        public void DeleteQuoteDocumentVersion(QuoteDocumentVersionPM quoteDocumentVersion)
//        {
//            if (objectContext == null)
//            {
//                objectContext = QuotesContext.GetContext(quoteDocumentVersion.Tenant);
//            }
//            QuoteDocumentVersionRepository quoteDocumentVersionRepository;
//            quoteDocumentVersionRepository = new QuoteDocumentVersionRepository(objectContext);
//            //DepartmentQuery departmentQuery = new DepartmentQuery(departmentRepository);
//            QuoteDocumentVersion entity = quoteDocumentVersionRepository.GetSingleQuoteDocumentVersion(quoteDocumentVersion.QuoteId, quoteDocumentVersion.Tenant, quoteDocumentVersion.VersionNumber);
//            quoteDocumentVersionRepository.Remove(entity);
//        }

//    }
//}