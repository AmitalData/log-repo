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
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        public ARPaymentStatusPM GetSingleARPaymentStatus(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            arPaymentStatusQuery = new ARPaymentStatusQuery(tenant);
            return arPaymentStatusQuery.GetSingleARPaymentStatusPM(code);
        }

        public void UpdateARPaymentStatusList(ARPaymentStatusList list)
        {

        }

        public ARPaymentStatusList GetSingleARPaymentStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ARPaymentStatusList entityList = null;
            aRPaymentStatusRepository = new ARPaymentStatusRepository(tenant);
            arPaymentStatusQuery = new ARPaymentStatusQuery(aRPaymentStatusRepository);
            ARPaymentStatus entity = aRPaymentStatusRepository.GetSingleARPaymentStatus(code);

            if (entity != null)
            {
                List<ARPaymentStatus> singleEntityList = new List<ARPaymentStatus>();
                singleEntityList.Add(entity);

                IQueryable<ARPaymentStatus> iQueryable = singleEntityList.AsQueryable();
                IQueryable<ARPaymentStatusList> iQueryableEntityList = arPaymentStatusQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<ARPaymentStatusList> GetARPaymentStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRPaymentStatusRepository = new ARPaymentStatusRepository(tenant);
            arPaymentStatusQuery = new ARPaymentStatusQuery(aRPaymentStatusRepository);
            IQueryable<ARPaymentStatus> iQueryable = aRPaymentStatusRepository.GetARPaymentStatus();
            IQueryable<ARPaymentStatusList> query2 = arPaymentStatusQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ARPaymentStatusList> GetARPaymentStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRPaymentStatusRepository = new ARPaymentStatusRepository(tenant);
            arPaymentStatusQuery = new ARPaymentStatusQuery(aRPaymentStatusRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARPaymentStatus> iQueryable = aRPaymentStatusRepository.GetARPaymentStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARPaymentStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ARPaymentStatusList> query2 = arPaymentStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARPaymentStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ARPaymentStatusList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<ARPaymentStatusList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<ARPaymentStatusList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<ARPaymentStatusList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<ARPaymentStatusList, int>(queryOperations, query2);
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

        public int GetARPaymentStatusCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aRPaymentStatusRepository = new ARPaymentStatusRepository(tenant);
            arPaymentStatusQuery = new ARPaymentStatusQuery(aRPaymentStatusRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARPaymentStatus> iQueryable = aRPaymentStatusRepository.GetARPaymentStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARPaymentStatus>(nonListQueryOperation, iQueryable);

            IQueryable<ARPaymentStatusList> query2 = arPaymentStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARPaymentStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}