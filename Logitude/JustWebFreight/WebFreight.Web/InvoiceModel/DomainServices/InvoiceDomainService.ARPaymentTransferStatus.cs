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
        private ARPaymentTransferStatusQuery aRPaymentTransferStatusQuery;
        private ARPaymentTransferStatusRepository aRPaymentTransferStatusRepository;

        public ARPaymentTransferStatusPM GetSingleARPaymentTransferStatus(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRPaymentTransferStatusQuery = new ARPaymentTransferStatusQuery(tenant);

            return aRPaymentTransferStatusQuery.GetSingleARPaymentTransferStatusPM(code);
        }

        public void UpdateARPaymentTransferStatusList(ARPaymentTransferStatusList entitylist)
        {

        }

        public ARPaymentTransferStatusList GetSingleARPaymentTransferStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ARPaymentTransferStatusList entityList = null;
            aRPaymentTransferStatusRepository = new ARPaymentTransferStatusRepository(tenant);
            ARPaymentTransferStatus entity = aRPaymentTransferStatusRepository.GetSingleARPaymentTransferStatus(code);
            aRPaymentTransferStatusQuery = new ARPaymentTransferStatusQuery(aRPaymentTransferStatusRepository);

            if (entity != null)
            {
                List<ARPaymentTransferStatus> SingleEntityList = new List<ARPaymentTransferStatus>();
                SingleEntityList.Add(entity);

                IQueryable<ARPaymentTransferStatus> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<ARPaymentTransferStatusList> iQueryableEntityList = aRPaymentTransferStatusQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<ARPaymentTransferStatusList> GetARPaymentTransferStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRPaymentTransferStatusRepository = new ARPaymentTransferStatusRepository(tenant);
            aRPaymentTransferStatusQuery = new ARPaymentTransferStatusQuery(aRPaymentTransferStatusRepository);

            IQueryable<ARPaymentTransferStatus> iQueryable = aRPaymentTransferStatusRepository.GetARPaymentTransferStatus();
            IQueryable<ARPaymentTransferStatusList> query2 = aRPaymentTransferStatusQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ARPaymentTransferStatusList> GetARPaymentTransferStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRPaymentTransferStatusRepository = new ARPaymentTransferStatusRepository(tenant);
            aRPaymentTransferStatusQuery = new ARPaymentTransferStatusQuery(aRPaymentTransferStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARPaymentTransferStatus> iQueryable = aRPaymentTransferStatusRepository.GetARPaymentTransferStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARPaymentTransferStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ARPaymentTransferStatusList> query2 = aRPaymentTransferStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARPaymentTransferStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ARPaymentTransferStatusList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<ARPaymentTransferStatusList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<ARPaymentTransferStatusList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<ARPaymentTransferStatusList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<ARPaymentTransferStatusList, int>(queryOperations, query2);
                            break;
                        }
                    case "boolean":
                        {
                            query2 = sortClass.GetSorterQuery<ARPaymentTransferStatusList, bool>(queryOperations, query2);
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

        public int GetARPaymentTransferStatusCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRPaymentTransferStatusRepository = new ARPaymentTransferStatusRepository(tenant);
            aRPaymentTransferStatusQuery = new ARPaymentTransferStatusQuery(aRPaymentTransferStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARPaymentTransferStatus> iQueryable = aRPaymentTransferStatusRepository.GetARPaymentTransferStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARPaymentTransferStatus>(nonListQueryOperation, iQueryable);

            IQueryable<ARPaymentTransferStatusList> query2 = aRPaymentTransferStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARPaymentTransferStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}