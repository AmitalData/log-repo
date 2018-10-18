using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;

using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        private ARInvoiceStatusRepository aRInvoiceStatusRepository;

        public ARInvoiceStatusPM GetSingleInvoiceStatus(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            arInvoiceStatusQuery = new ARInvoiceStatusQuery(tenant);

            return arInvoiceStatusQuery.GetSingleInvoiceStatusPM(code);
        }

        public void UpdateARInvoiceStatusList(ARInvoiceStatusList entitylist)
        {

        }

        public ARInvoiceStatusList GetSingleInvoiceStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ARInvoiceStatusList entityList = null;
            aRInvoiceStatusRepository = new ARInvoiceStatusRepository(tenant);
            ARInvoiceStatus entity = aRInvoiceStatusRepository.GetSingleARInvoiceStatus(code);
            arInvoiceStatusQuery = new ARInvoiceStatusQuery(aRInvoiceStatusRepository);

            if (entity != null)
            {
                List<ARInvoiceStatus> SingleEntityList = new List<ARInvoiceStatus>();
                SingleEntityList.Add(entity);

                IQueryable<ARInvoiceStatus> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<ARInvoiceStatusList> iQueryableEntityList = arInvoiceStatusQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<ARInvoiceStatusList> GetInvoiceStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRInvoiceStatusRepository = new ARInvoiceStatusRepository(tenant);
            arInvoiceStatusQuery = new ARInvoiceStatusQuery(aRInvoiceStatusRepository);

            IQueryable<ARInvoiceStatus> iQueryable = aRInvoiceStatusRepository.GetARInvoiceStatus();
            IQueryable<ARInvoiceStatusList> query2 = arInvoiceStatusQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ARInvoiceStatusList> GetARInvoiceStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRInvoiceStatusRepository = new ARInvoiceStatusRepository(tenant);
            arInvoiceStatusQuery = new ARInvoiceStatusQuery(aRInvoiceStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARInvoiceStatus> iQueryable = aRInvoiceStatusRepository.GetARInvoiceStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoiceStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ARInvoiceStatusList> query2 = arInvoiceStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoiceStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ARInvoiceStatusList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceStatusList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceStatusList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceStatusList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceStatusList, int>(queryOperations, query2);
                            break;
                        }
                    case "boolean":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceStatusList, bool>(queryOperations, query2);
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

        public int GetARInvoiceStatusCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRInvoiceStatusRepository = new ARInvoiceStatusRepository(tenant);
            arInvoiceStatusQuery = new ARInvoiceStatusQuery(aRInvoiceStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARInvoiceStatus> iQueryable = aRInvoiceStatusRepository.GetARInvoiceStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoiceStatus>(nonListQueryOperation, iQueryable);

            IQueryable<ARInvoiceStatusList> query2 = arInvoiceStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoiceStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}