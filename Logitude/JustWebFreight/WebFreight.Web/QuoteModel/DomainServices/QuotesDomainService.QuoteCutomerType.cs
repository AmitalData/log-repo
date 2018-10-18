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

using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.QuoteModel.DomainServices
{
    public partial class QuotesDomainService
    {
        private QuoteCustomerTypeRepository quoteCustomerTypeRepository;
        public IQueryable<QuoteCustomerType> GetQuoteCustomerTypesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteCustomerTypeRepository = new QuoteCustomerTypeRepository(tenant);
            return quoteCustomerTypeRepository.GetQuoteCustomerTypes();
        }

        public QuoteCustomerTypePM GetSingleQuoteCustomerTypePM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteCustomerTypeQuery = new QuoteCustomerTypeQuery(tenant);
            return quoteCustomerTypeQuery.GetSingleQuoteCustomerTypePM(code);
        }

        public QuoteCustomerType GetSingleQuoteCustomerType(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteCustomerTypeRepository = new QuoteCustomerTypeRepository(tenant);
            return quoteCustomerTypeRepository.GetSingleQuoteCustomerType(code);
        }

        public QuoteCustomerTypeList GetSingleQuoteCustomerTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteCustomerTypeRepository = new QuoteCustomerTypeRepository(tenant);
            quoteCustomerTypeQuery = new QuoteCustomerTypeQuery(tenant);
            QuoteCustomerTypeList quoteCustomerTypeList = null;
            QuoteCustomerType quoteCustomerType = quoteCustomerTypeRepository.GetSingleQuoteCustomerType(code);

            if (quoteCustomerType != null)
            {
                List<QuoteCustomerType> singleEntityList = new List<QuoteCustomerType>();
                singleEntityList.Add(quoteCustomerType);

                IQueryable<QuoteCustomerType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteCustomerTypeList> iQueryableEntityList = quoteCustomerTypeQuery.GetIQueryableEntityList(iQueryable);
                quoteCustomerTypeList = iQueryableEntityList.FirstOrDefault();
            }
            return quoteCustomerTypeList;
        }

        public IQueryable<QuoteCustomerTypeList> GetQuoteCustomerTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteCustomerTypeRepository = new QuoteCustomerTypeRepository(tenant);
            quoteCustomerTypeQuery = new QuoteCustomerTypeQuery(tenant);
            IQueryable<QuoteCustomerType> iQueryable = quoteCustomerTypeRepository.GetQuoteCustomerTypes();
            IQueryable<QuoteCustomerTypeList> query2 = quoteCustomerTypeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<QuoteCustomerTypeList> GetQuoteCustomerTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteCustomerTypeRepository = new QuoteCustomerTypeRepository(tenant);
            quoteCustomerTypeQuery = new QuoteCustomerTypeQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteCustomerType> iQueryable = quoteCustomerTypeRepository.GetQuoteCustomerTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<QuoteCustomerType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<QuoteCustomerTypeList> query2 = quoteCustomerTypeQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<QuoteCustomerTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteCustomerTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("PartnerType", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteCustomerTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteCustomerTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteCustomerTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteCustomerTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteCustomerTypeList, bool>(queryOperations, query2);
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

        public int GetQuoteCustomerTypeCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteCustomerTypeRepository = new QuoteCustomerTypeRepository(tenant);
            quoteCustomerTypeQuery = new QuoteCustomerTypeQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<QuoteCustomerType> iQueryable = quoteCustomerTypeRepository.GetQuoteCustomerTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<QuoteCustomerType>(nonListQueryOperation, iQueryable);

            IQueryable<QuoteCustomerTypeList> query2 = quoteCustomerTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<QuoteCustomerTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertQuoteCustomerType(QuoteCustomerType entity)
        {
            quoteCustomerTypeRepository.Add(entity);
        }

        public void UpdateQuoteCustomerType(QuoteCustomerType currentEntity)
        {
            quoteCustomerTypeRepository.Update(currentEntity);
        }

        public void DeleteQuoteCustomerType(QuoteCustomerType entity)
        {
            quoteCustomerTypeRepository.Remove(entity);
        }
    }
}