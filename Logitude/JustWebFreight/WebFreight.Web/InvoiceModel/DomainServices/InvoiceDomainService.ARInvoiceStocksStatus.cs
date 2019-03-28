using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        private ARInvoiceStocksStatusRepository ARInvoiceStocksStatusRepository;        
        
        public ARInvoiceStocksStatusList GetSingleInvoiceStocksStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ARInvoiceStocksStatusList entityList = null;
            ARInvoiceStocksStatusRepository = new ARInvoiceStocksStatusRepository(tenant);
            ARInvoiceStocksStatus entity = ARInvoiceStocksStatusRepository.GetSingleARInvoiceStocksStatus(code);
            ARInvoiceStocksStatusQuery aRInvoiceStocksStatusQuery = new ARInvoiceStocksStatusQuery(ARInvoiceStocksStatusRepository);

            if (entity != null)
            {
                List<ARInvoiceStocksStatus> SingleEntityList = new List<ARInvoiceStocksStatus>();
                SingleEntityList.Add(entity);

                IQueryable<ARInvoiceStocksStatus> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<ARInvoiceStocksStatusList> iQueryableEntityList = aRInvoiceStocksStatusQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<ARInvoiceStocksStatusList> GetInvoiceStocksStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ARInvoiceStocksStatusRepository = new ARInvoiceStocksStatusRepository(tenant);
            ARInvoiceStocksStatusQuery ARInvoiceStocksStatusQuery = new ARInvoiceStocksStatusQuery(ARInvoiceStocksStatusRepository);

            IQueryable<ARInvoiceStocksStatus> iQueryable = ARInvoiceStocksStatusRepository.GetARInvoiceStocksStatus();
            IQueryable<ARInvoiceStocksStatusList> query2 = ARInvoiceStocksStatusQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ARInvoiceStocksStatusList> GetARInvoiceStocksStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ARInvoiceStocksStatusRepository = new ARInvoiceStocksStatusRepository(tenant);
            ARInvoiceStocksStatusQuery ARInvoiceStocksStatusQuery = new ARInvoiceStocksStatusQuery(ARInvoiceStocksStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARInvoiceStocksStatus> iQueryable = ARInvoiceStocksStatusRepository.GetARInvoiceStocksStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoiceStocksStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ARInvoiceStocksStatusList> query2 = ARInvoiceStocksStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoiceStocksStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ARInvoiceStocksStatusList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceStocksStatusList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceStocksStatusList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceStocksStatusList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceStocksStatusList, int>(queryOperations, query2);
                            break;
                        }
                    case "boolean":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceStocksStatusList, bool>(queryOperations, query2);
                            break;
                        }
                    default:
                        {
                            query2 = query2.OrderByDescending(d => d.Code);
                            break;
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

        public int GetARInvoiceStocksStatusCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ARInvoiceStocksStatusRepository = new ARInvoiceStocksStatusRepository(tenant);
            ARInvoiceStocksStatusQuery ARInvoiceStocksStatusQuery = new ARInvoiceStocksStatusQuery(ARInvoiceStocksStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARInvoiceStocksStatus> iQueryable = ARInvoiceStocksStatusRepository.GetARInvoiceStocksStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoiceStocksStatus>(nonListQueryOperation, iQueryable);

            IQueryable<ARInvoiceStocksStatusList> query2 = ARInvoiceStocksStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoiceStocksStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}