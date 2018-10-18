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
        private APInvoiceTransferStatusQuery aPInvoiceTransferStatusQuery;
        private APInvoiceTransferStatusRepository aPInvoiceTransferStatusRepository;

        public APInvoiceTransferStatusPM GetSingleAPInvoiceTransferStatus(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPInvoiceTransferStatusQuery = new APInvoiceTransferStatusQuery(tenant);

            return aPInvoiceTransferStatusQuery.GetSingleAPInvoiceTransferStatusPM(code);
        }

        public void UpdateAPInvoiceTransferStatusList(APInvoiceTransferStatusList entitylist)
        {

        }

        public APInvoiceTransferStatusList GetSingleAPInvoiceTransferStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            APInvoiceTransferStatusList entityList = null;
            aPInvoiceTransferStatusRepository = new APInvoiceTransferStatusRepository(tenant);
            APInvoiceTransferStatus entity = aPInvoiceTransferStatusRepository.GetSingleAPInvoiceTransferStatus(code);
            aPInvoiceTransferStatusQuery = new APInvoiceTransferStatusQuery(aPInvoiceTransferStatusRepository);

            if (entity != null)
            {
                List<APInvoiceTransferStatus> SingleEntityList = new List<APInvoiceTransferStatus>();
                SingleEntityList.Add(entity);

                IQueryable<APInvoiceTransferStatus> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<APInvoiceTransferStatusList> iQueryableEntityList = aPInvoiceTransferStatusQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<APInvoiceTransferStatusList> GetAPInvoiceTransferStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPInvoiceTransferStatusRepository = new APInvoiceTransferStatusRepository(tenant);
            aPInvoiceTransferStatusQuery = new APInvoiceTransferStatusQuery(aPInvoiceTransferStatusRepository);

            IQueryable<APInvoiceTransferStatus> iQueryable = aPInvoiceTransferStatusRepository.GetAPInvoiceTransferStatus();
            IQueryable<APInvoiceTransferStatusList> query2 = aPInvoiceTransferStatusQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<APInvoiceTransferStatusList> GetAPInvoiceTransferStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPInvoiceTransferStatusRepository = new APInvoiceTransferStatusRepository(tenant);
            aPInvoiceTransferStatusQuery = new APInvoiceTransferStatusQuery(aPInvoiceTransferStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<APInvoiceTransferStatus> iQueryable = aPInvoiceTransferStatusRepository.GetAPInvoiceTransferStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APInvoiceTransferStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<APInvoiceTransferStatusList> query2 = aPInvoiceTransferStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<APInvoiceTransferStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(APInvoiceTransferStatusList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<APInvoiceTransferStatusList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<APInvoiceTransferStatusList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<APInvoiceTransferStatusList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<APInvoiceTransferStatusList, int>(queryOperations, query2);
                            break;
                        }
                    case "boolean":
                        {
                            query2 = sortClass.GetSorterQuery<APInvoiceTransferStatusList, bool>(queryOperations, query2);
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

        public int GetAPInvoiceTransferStatusCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPInvoiceTransferStatusRepository = new APInvoiceTransferStatusRepository(tenant);
            aPInvoiceTransferStatusQuery = new APInvoiceTransferStatusQuery(aPInvoiceTransferStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<APInvoiceTransferStatus> iQueryable = aPInvoiceTransferStatusRepository.GetAPInvoiceTransferStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APInvoiceTransferStatus>(nonListQueryOperation, iQueryable);

            IQueryable<APInvoiceTransferStatusList> query2 = aPInvoiceTransferStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<APInvoiceTransferStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}