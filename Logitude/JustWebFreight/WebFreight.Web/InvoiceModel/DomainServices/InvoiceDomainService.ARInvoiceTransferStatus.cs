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
using System.Web;
using System.Xml.Serialization;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using WebFreight.Web.Security;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        private ARInvoiceTransferStatusQuery aRInvoiceTransferStatusQuery;
        private ARInvoiceTransferStatusRepository aRInvoiceTransferStatusRepository;

        public ARInvoiceTransferStatusPM GetSingleARInvoiceTransferStatus(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRInvoiceTransferStatusQuery = new ARInvoiceTransferStatusQuery(tenant);

            return aRInvoiceTransferStatusQuery.GetSingleARInvoiceTransferStatusPM(code);
        }

        public void UpdateARInvoiceTransferStatusList(ARInvoiceTransferStatusList entitylist)
        {

        }

        public ARInvoiceTransferStatusList GetSingleARInvoiceTransferStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ARInvoiceTransferStatusList entityList = null;
            aRInvoiceTransferStatusRepository = new ARInvoiceTransferStatusRepository(tenant);
            ARInvoiceTransferStatus entity = aRInvoiceTransferStatusRepository.GetSingleARInvoiceTransferStatus(code);
            aRInvoiceTransferStatusQuery = new ARInvoiceTransferStatusQuery(aRInvoiceTransferStatusRepository);

            if (entity != null)
            {
                List<ARInvoiceTransferStatus> SingleEntityList = new List<ARInvoiceTransferStatus>();
                SingleEntityList.Add(entity);

                IQueryable<ARInvoiceTransferStatus> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<ARInvoiceTransferStatusList> iQueryableEntityList = aRInvoiceTransferStatusQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<ARInvoiceTransferStatusList> GetARInvoiceTransferStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRInvoiceTransferStatusRepository = new ARInvoiceTransferStatusRepository(tenant);
            aRInvoiceTransferStatusQuery = new ARInvoiceTransferStatusQuery(aRInvoiceTransferStatusRepository);

            IQueryable<ARInvoiceTransferStatus> iQueryable = aRInvoiceTransferStatusRepository.GetARInvoiceTransferStatus();
            IQueryable<ARInvoiceTransferStatusList> query2 = aRInvoiceTransferStatusQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ARInvoiceTransferStatusList> GetARInvoiceTransferStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRInvoiceTransferStatusRepository = new ARInvoiceTransferStatusRepository(tenant);
            aRInvoiceTransferStatusQuery = new ARInvoiceTransferStatusQuery(aRInvoiceTransferStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARInvoiceTransferStatus> iQueryable = aRInvoiceTransferStatusRepository.GetARInvoiceTransferStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoiceTransferStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ARInvoiceTransferStatusList> query2 = aRInvoiceTransferStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoiceTransferStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ARInvoiceTransferStatusList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceTransferStatusList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceTransferStatusList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceTransferStatusList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceTransferStatusList, int>(queryOperations, query2);
                            break;
                        }
                    case "boolean":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceTransferStatusList, bool>(queryOperations, query2);
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

        public int GetARInvoiceTransferStatusCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRInvoiceTransferStatusRepository = new ARInvoiceTransferStatusRepository(tenant);
            aRInvoiceTransferStatusQuery = new ARInvoiceTransferStatusQuery(aRInvoiceTransferStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARInvoiceTransferStatus> iQueryable = aRInvoiceTransferStatusRepository.GetARInvoiceTransferStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoiceTransferStatus>(nonListQueryOperation, iQueryable);

            IQueryable<ARInvoiceTransferStatusList> query2 = aRInvoiceTransferStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoiceTransferStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}