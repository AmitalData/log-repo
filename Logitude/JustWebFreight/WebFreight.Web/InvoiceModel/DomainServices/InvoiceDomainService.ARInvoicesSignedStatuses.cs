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
        private ARInvoicesSignedStatusRepository aRInvoicesSignedStatusRepository;

        public ARInvoicesSignedStatusPM GetSingleInvoicesSignedStatus(string code, int tenant)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                throw new Exception("Authentication failed: " + ex.Message);
            }


            aRInvoicesSignedStatusQuery = new ARInvoicesSignedStatusQuery(tenant);

            return aRInvoicesSignedStatusQuery.GetSingleInvoicesSignedStatusPM(code);
        }

        public void UpdateARInvoicesSignedStatusList(ARInvoicesSignedStatusList entitylist)
        {

        }

        public ARInvoicesSignedStatusList GetSingleInvoicesSignedStatusList(string code, int tenant)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                throw new Exception("Authentication failed: " + ex.Message);
            }

            ARInvoicesSignedStatusList entityList = null;
            aRInvoicesSignedStatusRepository = new ARInvoicesSignedStatusRepository(tenant);
            ARInvoicesSignedStatus entity = aRInvoicesSignedStatusRepository.GetSingleARInvoicesSignedStatus(code);
            aRInvoicesSignedStatusQuery = new ARInvoicesSignedStatusQuery(aRInvoicesSignedStatusRepository);

            if (entity != null)
            {
                List<ARInvoicesSignedStatus> SingleEntityList = new List<ARInvoicesSignedStatus>();
                SingleEntityList.Add(entity);

                IQueryable<ARInvoicesSignedStatus> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<ARInvoicesSignedStatusList> iQueryableEntityList = aRInvoicesSignedStatusQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<ARInvoicesSignedStatusList> GetInvoicesSignedStatusLists(int tenant)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                throw new Exception("Authentication failed: " + ex.Message);
            }

            aRInvoicesSignedStatusRepository = new ARInvoicesSignedStatusRepository(tenant);
            aRInvoicesSignedStatusQuery = new ARInvoicesSignedStatusQuery(aRInvoicesSignedStatusRepository);

            IQueryable<ARInvoicesSignedStatus> iQueryable = aRInvoicesSignedStatusRepository.GetARInvoicesSignedStatuses();
            IQueryable<ARInvoicesSignedStatusList> query2 = aRInvoicesSignedStatusQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ARInvoicesSignedStatusList> GetARInvoicesSignedStatusFilters(byte[] xmlFilters, int tenant)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                throw new Exception("Authentication failed: " + ex.Message);
            }

            aRInvoicesSignedStatusRepository = new ARInvoicesSignedStatusRepository(tenant);
            aRInvoicesSignedStatusQuery = new ARInvoicesSignedStatusQuery(aRInvoicesSignedStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            if (queryOperations == null) throw new InvalidOperationException("Failed to deserialize filters.");
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARInvoicesSignedStatus> iQueryable = aRInvoicesSignedStatusRepository.GetARInvoicesSignedStatuses();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoicesSignedStatus>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ARInvoicesSignedStatusList> query2 = aRInvoicesSignedStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoicesSignedStatusList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ARInvoicesSignedStatusList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoicesSignedStatusList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoicesSignedStatusList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoicesSignedStatusList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoicesSignedStatusList, int>(queryOperations, query2);
                            break;
                        }
                    case "boolean":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoicesSignedStatusList, bool>(queryOperations, query2);
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

        public int GetARInvoicesSignedStatusCount(byte[] xmlFilters, int tenant)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
                throw new Exception("Authentication failed: " + ex.Message);
            }

            aRInvoicesSignedStatusRepository = new ARInvoicesSignedStatusRepository(tenant);
            aRInvoicesSignedStatusQuery = new ARInvoicesSignedStatusQuery(aRInvoicesSignedStatusRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            if (queryOperations == null) throw new InvalidOperationException("Failed to deserialize filters.");
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARInvoicesSignedStatus> iQueryable = aRInvoicesSignedStatusRepository.GetARInvoicesSignedStatuses();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoicesSignedStatus>(nonListQueryOperation, iQueryable);

            IQueryable<ARInvoicesSignedStatusList> query2 = aRInvoicesSignedStatusQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoicesSignedStatusList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}