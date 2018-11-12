using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
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
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {

        private APPaymentTransferStatusQuery aPPaymentTransferStatusQuery;
        private APPaymentTransferStatusRepository aPPaymentTransferStatusRepository;

        public APPaymentTransferStatusPM GetSingleAPPaymentTransferStatus(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPPaymentTransferStatusQuery = new APPaymentTransferStatusQuery(tenant);

            return aPPaymentTransferStatusQuery.GetSingleAPPaymentTransferStatusPM(code);
        }

        public void UpdateAPPaymentTransferStatusList(APPaymentTransferStatusList entitylist)
        {

        }

        public APPaymentTransferStatusList GetSingleAPPaymentTransferStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            APPaymentTransferStatusList entityList = null;
            aPPaymentTransferStatusRepository = new APPaymentTransferStatusRepository(tenant);
            APPaymentTransferStatus entity = aPPaymentTransferStatusRepository.GetSingleAPPaymentTransferStatus(code);
            aPPaymentTransferStatusQuery = new APPaymentTransferStatusQuery(aPPaymentTransferStatusRepository);

            if (entity != null)
            {
                List<APPaymentTransferStatus> SingleEntityList = new List<APPaymentTransferStatus>();
                SingleEntityList.Add(entity);

                IQueryable<APPaymentTransferStatus> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<APPaymentTransferStatusList> iQueryableEntityList = aPPaymentTransferStatusQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<APPaymentTransferStatusList> GetAPPaymentTransferStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPPaymentTransferStatusRepository = new APPaymentTransferStatusRepository(tenant);
            aPPaymentTransferStatusQuery = new APPaymentTransferStatusQuery(aPPaymentTransferStatusRepository);

            IQueryable<APPaymentTransferStatus> iQueryable = aPPaymentTransferStatusRepository.GetAPPaymentTransferStatus();
            IQueryable<APPaymentTransferStatusList> query2 = aPPaymentTransferStatusQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<APPaymentTransferStatusList> GetAPPaymentTransferStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPPaymentTransferStatusRepository = new APPaymentTransferStatusRepository(tenant);
            aPPaymentTransferStatusQuery = new APPaymentTransferStatusQuery(aPPaymentTransferStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<APPaymentTransferStatus> iQueryable = aPPaymentTransferStatusRepository.GetAPPaymentTransferStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APPaymentTransferStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<APPaymentTransferStatusList> query2 = aPPaymentTransferStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<APPaymentTransferStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(APPaymentTransferStatusList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<APPaymentTransferStatusList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<APPaymentTransferStatusList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<APPaymentTransferStatusList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<APPaymentTransferStatusList, int>(queryOperations, query2);
                            break;
                        }
                    case "boolean":
                        {
                            query2 = sortClass.GetSorterQuery<APPaymentTransferStatusList, bool>(queryOperations, query2);
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



        public int GetAPPaymentTransferStatusCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPPaymentTransferStatusRepository = new APPaymentTransferStatusRepository(tenant);
            aPPaymentTransferStatusQuery = new APPaymentTransferStatusQuery(aPPaymentTransferStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<APPaymentTransferStatus> iQueryable = aPPaymentTransferStatusRepository.GetAPPaymentTransferStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APPaymentTransferStatus>(nonListQueryOperation, iQueryable);

            IQueryable<APPaymentTransferStatusList> query2 = aPPaymentTransferStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<APPaymentTransferStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}