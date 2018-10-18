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
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.Security;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
	{
        public void UpdateARInvoiceLineActionList(ARInvoiceLineActionList entitylist)
        {

        }

        public ARInvoiceLineActionList GetSingleARInvoiceLineActionList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ARInvoiceLineActionList entityList = null;
            ARInvoiceLineActionRepository entityRepository = new ARInvoiceLineActionRepository(tenant);
            ARInvoiceLineActionQuery entityQuery = new ARInvoiceLineActionQuery(entityRepository);
            ARInvoiceLineAction entity = entityRepository.GetSingleARInvoiceLineAction(code);

            if (entity != null)
            {
                List<ARInvoiceLineAction> SingleEntityList = new List<ARInvoiceLineAction>();
                SingleEntityList.Add(entity);

                IQueryable<ARInvoiceLineAction> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<ARInvoiceLineActionList> iQueryableEntityList = entityQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<ARInvoiceLineActionList> GetARInvoiceLineActionLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ARInvoiceLineActionRepository entityRepository = new ARInvoiceLineActionRepository(tenant);
            ARInvoiceLineActionQuery entityQuery = new ARInvoiceLineActionQuery(entityRepository);

            IQueryable<ARInvoiceLineAction> iQueryable = entityRepository.GetARInvoiceLineActions();
            IQueryable<ARInvoiceLineActionList> query2 = entityQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ARInvoiceLineActionList> GetARInvoiceLineActionFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ARInvoiceLineActionRepository entityRepository = new ARInvoiceLineActionRepository(tenant);
            ARInvoiceLineActionQuery entityQuery = new ARInvoiceLineActionQuery(entityRepository);
            IQueryable<ARInvoiceLineAction> iQueryable = entityRepository.GetARInvoiceLineActions();

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoiceLineAction>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ARInvoiceLineActionList> query2 = entityQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoiceLineActionList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ARInvoiceLineActionList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceLineActionList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceLineActionList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceLineActionList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceLineActionList, int>(queryOperations, query2);
                            break;
                        }
                    case "boolean":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceLineActionList, bool>(queryOperations, query2);
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

        public int GetARInvoiceLineActionCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            ARInvoiceLineActionRepository entityRepository = new ARInvoiceLineActionRepository(tenant);
            ARInvoiceLineActionQuery entityQuery = new ARInvoiceLineActionQuery(entityRepository);
            IQueryable<ARInvoiceLineAction> iQueryable = entityRepository.GetARInvoiceLineActions();

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();

            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoiceLineAction>(nonListQueryOperation, iQueryable);

            IQueryable<ARInvoiceLineActionList> query2 = entityQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoiceLineActionList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
	}
}