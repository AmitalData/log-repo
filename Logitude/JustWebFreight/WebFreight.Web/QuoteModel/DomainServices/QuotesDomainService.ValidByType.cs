using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityQueries;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;

namespace WebFreight.Web.QuoteModel.DomainServices
{
    public partial class QuotesDomainService
    {
        private ValidByTypeRepository validByTypeRepository;
        public IQueryable<ValidByType> GetValidByTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            validByTypeRepository = new ValidByTypeRepository(tenant);
            return validByTypeRepository.GetValidByTypes();
        }

        public ValidByType GetSingleValidByType(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            validByTypeRepository = new ValidByTypeRepository(tenant);
            return validByTypeRepository.GetSingleValidByType(code);
        }

        public ValidByTypeList GetSingleValidByTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            validByTypeRepository = new ValidByTypeRepository(tenant);
            validByTypeQuery = new ValidByTypeQuery(tenant);

            ValidByTypeList validByTypeList = null;
            ValidByType validByType = validByTypeRepository.GetSingleValidByType(code);

            if (validByType != null)
            {
                List<ValidByType> singleEntityList = new List<ValidByType>();
                singleEntityList.Add(validByType);

                IQueryable<ValidByType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<ValidByTypeList> iQueryableEntityList = validByTypeQuery.GetIQueryableEntityList(iQueryable);
                validByTypeList = iQueryableEntityList.FirstOrDefault();
            }
            return validByTypeList;
        }

        public IQueryable<ValidByTypeList> GetValidByTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            validByTypeRepository = new ValidByTypeRepository(tenant);
            validByTypeQuery = new ValidByTypeQuery(tenant);
            IQueryable<ValidByType> iQueryable = validByTypeRepository.GetValidByTypes();
            IQueryable<ValidByTypeList> query2 = validByTypeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ValidByTypeList> GetValidByTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            validByTypeRepository = new ValidByTypeRepository(tenant);
            validByTypeQuery = new ValidByTypeQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ValidByType> iQueryable = validByTypeRepository.GetValidByTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ValidByType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<ValidByTypeList> query2 = validByTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<ValidByTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ValidByTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> validByTypeObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ValidByType", tenant).ToList();

                ObjectField objectField = (from a in validByTypeObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ValidByTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ValidByTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ValidByTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ValidByTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ValidByTypeList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetValidByTypeCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            validByTypeRepository = new ValidByTypeRepository(tenant);
            validByTypeQuery = new ValidByTypeQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ValidByType> iQueryable = validByTypeRepository.GetValidByTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ValidByType>(nonListQueryOperation, iQueryable);

            IQueryable<ValidByTypeList> query2 = validByTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<ValidByTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertValidByType(ValidByType entity)
        {
            validByTypeRepository.Add(entity);
        }

        public void UpdateValidByType(ValidByType currentEntity)
        {
            validByTypeRepository.Update(currentEntity);
        }

        public void DeleteValidByType(ValidByType entity)
        {
            validByTypeRepository.Remove(entity);
        }
    }
}