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
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using WebFreight.Web.Security;

namespace WebFreight.Web.QuoteModel.DomainServices
{
    public partial class QuotesDomainService
    {
        private QuoteClosingReasonRepository quoteClosingReasonRepository;

        public IQueryable<QuoteClosingReason> GetQuoteClosingReasons(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteClosingReasonRepository = new QuoteClosingReasonRepository(tenant);
            return quoteClosingReasonRepository.GetQuoteClosingReasons();
        }

        public QuoteClosingReason GetSingleQuoteClosingReason(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteClosingReasonRepository = new QuoteClosingReasonRepository(tenant);
            return quoteClosingReasonRepository.GetSingleQuoteClosingReason(code);
        }

        public QuoteClosingReasonPM GetSingleQuoteClosingReasonPM(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteClosingReasonQuery = new QuoteClosingReasonQuery(tenant);
            return quoteClosingReasonQuery.GetSingleQuoteClosingReasonPM(code);
        }
        
        public QuoteClosingReasonList GetSingleQuoteClosingReasonList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteClosingReasonRepository = new QuoteClosingReasonRepository(tenant);
            quoteClosingReasonQuery = new QuoteClosingReasonQuery(tenant);
            QuoteClosingReasonList quoteClosingReasonList = null;
            QuoteClosingReason quoteClosingReason = quoteClosingReasonRepository.GetSingleQuoteClosingReason(code);

            if (quoteClosingReason != null)
            {
                List<QuoteClosingReason> singleEntityList = new List<QuoteClosingReason>();
                singleEntityList.Add(quoteClosingReason);

                IQueryable<QuoteClosingReason> iQueryable = singleEntityList.AsQueryable();
                IQueryable<QuoteClosingReasonList> iQueryableEntityList = quoteClosingReasonQuery.GetIQueryableEntityList(iQueryable);
                quoteClosingReasonList = iQueryableEntityList.FirstOrDefault();
            }
            return quoteClosingReasonList;
        }

        public IQueryable<QuoteClosingReasonList> GetQuoteClosingReasonLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteClosingReasonRepository = new QuoteClosingReasonRepository(tenant);
            quoteClosingReasonQuery = new QuoteClosingReasonQuery(tenant);
            IQueryable<QuoteClosingReason> iQueryable = quoteClosingReasonRepository.GetQuoteClosingReasons();
            IQueryable<QuoteClosingReasonList> query2 = quoteClosingReasonQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<QuoteClosingReasonList> GetQuoteClosingReasonFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteClosingReasonRepository = new QuoteClosingReasonRepository(tenant);
            quoteClosingReasonQuery = new QuoteClosingReasonQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteClosingReason> iQueryable = quoteClosingReasonRepository.GetQuoteClosingReasons();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<QuoteClosingReason>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<QuoteClosingReasonList> query2 = quoteClosingReasonQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<QuoteClosingReasonList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(QuoteClosingReasonList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("QuoteClosingReason", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteClosingReasonList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteClosingReasonList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteClosingReasonList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteClosingReasonList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<QuoteClosingReasonList, bool>(queryOperations, query2);
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

        public int GetQuoteClosingReasonFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            quoteClosingReasonRepository = new QuoteClosingReasonRepository(tenant);
            quoteClosingReasonQuery = new QuoteClosingReasonQuery(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<QuoteClosingReason> iQueryable = quoteClosingReasonRepository.GetQuoteClosingReasons();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<QuoteClosingReason>(nonListQueryOperation, iQueryable);

            IQueryable<QuoteClosingReasonList> query2 = quoteClosingReasonQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<QuoteClosingReasonList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertQuoteClosingReason(QuoteClosingReason entity)
        {
            quoteClosingReasonRepository.Add(entity);
        }

        public void UpdateQuoteClosingReason(QuoteClosingReason currentEntity)
        {
            quoteClosingReasonRepository.Update(currentEntity);
        }

        public void DeleteQuoteClosingReason(QuoteClosingReason entity)
        {
            quoteClosingReasonRepository.Remove(entity);
        }
    }
}