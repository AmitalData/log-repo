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
        public APPaymentStatusPM GetSingleAPPaymentStatus(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            apPaymentStatusQuery = new APPaymentStatusQuery(tenant);
            return apPaymentStatusQuery.GetSingleAPPaymentStatusPM(code);
        }

        public void UpdateAPPaymentStatusList(APPaymentStatusList list)
        {

        }

        public APPaymentStatusList GetSingleAPPaymentStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            APPaymentStatusList entityList = null;
            aPPaymentStatusRepository = new APPaymentStatusRepository(tenant);
            apPaymentStatusQuery = new APPaymentStatusQuery(aPPaymentStatusRepository);
            APPaymentStatus entity = aPPaymentStatusRepository.GetSingleAPPaymentStatus(code);
            if (entity != null)
            {
                List<APPaymentStatus> singleEntityList = new List<APPaymentStatus>();
                singleEntityList.Add(entity);

                IQueryable<APPaymentStatus> iQueryable = singleEntityList.AsQueryable();
                IQueryable<APPaymentStatusList> iQueryableEntityList = apPaymentStatusQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<APPaymentStatusList> GetAPPaymentStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPPaymentStatusRepository = new APPaymentStatusRepository(tenant);
            apPaymentStatusQuery = new APPaymentStatusQuery(aPPaymentStatusRepository);
            IQueryable<APPaymentStatus> iQueryable = aPPaymentStatusRepository.GetAPPaymentStatus();
            IQueryable<APPaymentStatusList> query2 = apPaymentStatusQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<APPaymentStatusList> GetAPPaymentStatusFilters(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPPaymentStatusRepository = new APPaymentStatusRepository(tenant);
            apPaymentStatusQuery = new APPaymentStatusQuery(aPPaymentStatusRepository);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<APPaymentStatus> iQueryable = aPPaymentStatusRepository.GetAPPaymentStatus();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APPaymentStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<APPaymentStatusList> query2 = apPaymentStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<APPaymentStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(APPaymentStatusList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<APPaymentStatusList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<APPaymentStatusList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<APPaymentStatusList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<APPaymentStatusList, int>(queryOperations, query2);
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

        public int GetAPPaymentStatusCount(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPPaymentStatusRepository = new APPaymentStatusRepository(tenant);
            apPaymentStatusQuery = new APPaymentStatusQuery(aPPaymentStatusRepository);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<APPaymentStatus> iQueryable = aPPaymentStatusRepository.GetAPPaymentStatus();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APPaymentStatus>(nonListQueryOperation, iQueryable);

            IQueryable<APPaymentStatusList> query2 = apPaymentStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<APPaymentStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}