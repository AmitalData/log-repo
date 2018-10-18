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
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;

namespace WebFreight.Web.QuoteModel.DomainServices
{
    public partial class QuotesDomainService
    {
        private QuoteTypeRepository quoteTypeRepository;
        public IQueryable<QuoteType> GetQuoteTypes(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteTypeRepository = new QuoteTypeRepository(tenant);
            return quoteTypeRepository.GetQuoteTypes();
        }

        public QuoteType GetSingleQuoteType(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteTypeRepository = new QuoteTypeRepository(tenant);
            return quoteTypeRepository.GetSingleQuoteType(code);
        }

        public QuoteTypePM GetSingleQuoteTypePM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("QuoteType", "READ", tenant);

            quoteTypeQuery = new QuoteTypeQuery(tenant);
            return quoteTypeQuery.GetSingleQuoteTypePM(code);
        }

        public QuoteTypeList GetSingleQuoteTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteTypeRepository = new QuoteTypeRepository(tenant);
            quoteTypeQuery = new QuoteTypeQuery(tenant);
            QuoteTypeList quoteTypeList = null;
            QuoteType quoteType = quoteTypeRepository.GetSingleQuoteType(code);

            if (quoteType != null)
            {
                List<QuoteType> singleEntityList = new List<QuoteType>();
                singleEntityList.Add(quoteType);

                IQueryable<QuoteType> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteTypeList> iQueryableEntityList = quoteTypeQuery.GetIQueryableEntityList(iQueryable);
                quoteTypeList = iQueryableEntityList.FirstOrDefault();
            }
            return quoteTypeList;
        }

        public IQueryable<QuoteTypeList> GetQuoteTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteTypeRepository = new QuoteTypeRepository(tenant);
            quoteTypeQuery = new QuoteTypeQuery(tenant);
            IQueryable<QuoteType> iQueryable = quoteTypeRepository.GetQuoteTypes();
            IQueryable<QuoteTypeList> query2 = quoteTypeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<QuoteTypeList> GetQuoteTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteTypeRepository = new QuoteTypeRepository(tenant);
            quoteTypeQuery = new QuoteTypeQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteType> iQueryable = quoteTypeRepository.GetQuoteTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<QuoteType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<QuoteTypeList> query2 = quoteTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<QuoteTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteTypeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TransportMode", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTypeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTypeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTypeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTypeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteTypeList, bool>(queryOperations, query2);
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

        public int GetQuoteTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteTypeRepository = new QuoteTypeRepository(tenant);
            quoteTypeQuery = new QuoteTypeQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteType> iQueryable = quoteTypeRepository.GetQuoteTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<QuoteType>(nonListQueryOperation, iQueryable);

            IQueryable<QuoteTypeList> query2 = quoteTypeQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<QuoteTypeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertQuoteType(QuoteType entity)
        {
            quoteTypeRepository.Add(entity);
        }

        public void UpdateQuoteType(QuoteType currentEntity)
        {
            quoteTypeRepository.Update(currentEntity);
        }

        public void DeleteQuoteType(QuoteType entity)
        {
            quoteTypeRepository.Remove(entity);
        }
    }
}