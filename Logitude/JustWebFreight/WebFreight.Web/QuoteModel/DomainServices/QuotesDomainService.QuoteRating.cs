using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityQueries;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
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
using WebFreight.Web.Security;

namespace WebFreight.Web.QuoteModel.DomainServices
{
    public partial class QuotesDomainService
    {
        private QuoteRatingRepository QuoteRatingRepository;
        private QuoteRatingQuery QuoteRatingQuery;

        public IQueryable<QuoteRating> GetQuoteRatings(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            QuoteRatingRepository = new QuoteRatingRepository(tenant);
            return QuoteRatingRepository.GetQuoteRatings();
        }

        public QuoteRating GetSingleQuoteRating(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            QuoteRatingRepository = new QuoteRatingRepository(tenant);
            return QuoteRatingRepository.GetSingleQuoteRating(code);
        }

        public QuoteRatingList GetSingleQuoteRatingList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            QuoteRatingRepository = new QuoteRatingRepository(tenant);
            QuoteRatingQuery = new QuoteRatingQuery(tenant);
            QuoteRatingList QuoteRatingList = null;
            QuoteRating QuoteRating = QuoteRatingRepository.GetSingleQuoteRating(code);

            if (QuoteRating != null)
            {
                List<QuoteRating> singleEntityList = new List<QuoteRating>();
                singleEntityList.Add(QuoteRating);

                IQueryable<QuoteRating> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteRatingList> iQueryableEntityList = QuoteRatingQuery.GetIQueryableEntityList(iQueryable);
                QuoteRatingList = iQueryableEntityList.FirstOrDefault();
            }
            return QuoteRatingList;
        }

        public IQueryable<QuoteRatingList> GetQuoteRatingLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            QuoteRatingRepository = new QuoteRatingRepository(tenant);
            QuoteRatingQuery = new QuoteRatingQuery(tenant);
            IQueryable<QuoteRating> iQueryable = QuoteRatingRepository.GetQuoteRatings();
            IQueryable<QuoteRatingList> query2 = QuoteRatingQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<QuoteRatingList> GetQuoteRatingFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            QuoteRatingRepository = new QuoteRatingRepository(tenant);
            QuoteRatingQuery = new QuoteRatingQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteRating> iQueryable = QuoteRatingRepository.GetQuoteRatings();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<QuoteRating>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<QuoteRatingList> query2 = QuoteRatingQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<QuoteRatingList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteRatingList).GetProperty(queryOperations.SortByColumnName);
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
                                query2 = sortClass.GetSorterQuery<QuoteRatingList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteRatingList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteRatingList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteRatingList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteRatingList, bool>(queryOperations, query2);
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

        public int GetQuoteRatingFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            QuoteRatingRepository = new QuoteRatingRepository(tenant);
            QuoteRatingQuery = new QuoteRatingQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteRating> iQueryable = QuoteRatingRepository.GetQuoteRatings();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<QuoteRating>(nonListQueryOperation, iQueryable);

            IQueryable<QuoteRatingList> query2 = QuoteRatingQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<QuoteRatingList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}