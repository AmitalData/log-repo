using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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
using WebFreight.Web.Security;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateDocumentTypeCategoryList(DocumentTypeCategoryList currentEntity)
        {
        }

        public IQueryable<DocumentTypeCategory> GetDocumentTypeCategories(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeCategoryRepository = new DocumentTypeCategoryRepository(tenant);
            return documentTypeCategoryRepository.GetDocumentTypeCategories();
        }

        public IQueryable<DocumentTypeCategory> DocumentTypeCategoriesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeCategoryRepository = new DocumentTypeCategoryRepository(tenant);
            return documentTypeCategoryRepository.GetDocumentTypeCategories();
        }

        public DocumentTypeCategoryPM GetSingleDocumentTypeCategory(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeCategoryQuery = new DocumentTypeCategoryQuery(tenant);
            return documentTypeCategoryQuery.GetSinglePM(code);
        }

        public DocumentTypeCategoryList GetSingleDocumentTypeCategoryList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeCategoryRepository = new DocumentTypeCategoryRepository(tenant);
            DocumentTypeCategoryList documentTypeCategoryList = null;
            DocumentTypeCategory documentTypeCategory = documentTypeCategoryRepository.GetSingleDocumentTypeCategory(code);

            if (documentTypeCategory != null)
            {
                List<DocumentTypeCategory> singleEntityList = new List<DocumentTypeCategory>();
                singleEntityList.Add(documentTypeCategory);

                documentTypeCategoryQuery = new DocumentTypeCategoryQuery(documentTypeCategoryRepository);
                IQueryable<DocumentTypeCategory> iQueryable = singleEntityList.AsQueryable();
                IQueryable<DocumentTypeCategoryList> iQueryableEntityList = documentTypeCategoryQuery.GetIQueryableEntityList(iQueryable);
                documentTypeCategoryList = iQueryableEntityList.FirstOrDefault();
            }
            return documentTypeCategoryList;
        }

        public IQueryable<DocumentTypeCategoryList> GetDocumentTypeCategoryLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeCategoryRepository = new DocumentTypeCategoryRepository(tenant);
            documentTypeCategoryQuery = new DocumentTypeCategoryQuery(documentTypeCategoryRepository);

            IQueryable<DocumentTypeCategory> iQueryable = documentTypeCategoryRepository.GetDocumentTypeCategories();
            IQueryable<DocumentTypeCategoryList> query2 = documentTypeCategoryQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<DocumentTypeCategoryList> GetDocumentTypeCategoryFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeCategoryRepository = new DocumentTypeCategoryRepository(tenant);
            documentTypeCategoryQuery = new DocumentTypeCategoryQuery(documentTypeCategoryRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DocumentTypeCategory> iQueryable = documentTypeCategoryRepository.GetDocumentTypeCategories();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DocumentTypeCategory>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<DocumentTypeCategoryList> query2 = documentTypeCategoryQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DocumentTypeCategoryList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DocumentTypeCategoryList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("DocumentTypeCategory", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentTypeCategoryList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentTypeCategoryList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentTypeCategoryList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentTypeCategoryList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<DocumentTypeCategoryList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetDocumentTypeCategoryFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            documentTypeCategoryRepository = new DocumentTypeCategoryRepository(tenant);
            documentTypeCategoryQuery = new DocumentTypeCategoryQuery(documentTypeCategoryRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<DocumentTypeCategory> iQueryable = documentTypeCategoryRepository.GetDocumentTypeCategories();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DocumentTypeCategory>(nonListQueryOperation, iQueryable);

            IQueryable<DocumentTypeCategoryList> query2 = documentTypeCategoryQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<DocumentTypeCategoryList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public void InsertDocumentTypeCategory(DocumentTypeCategory entity)
        {
            documentTypeCategoryRepository.Add(entity);
        }

        public void UpdateDocumentTypeCategory(DocumentTypeCategory currentEntity)
        {
            documentTypeCategoryRepository.Update(currentEntity);
        }

        public void DeleteDocumentTypeCategory(DocumentTypeCategory entity)
        {
            documentTypeCategoryRepository.Remove(entity);
        }
    }
}