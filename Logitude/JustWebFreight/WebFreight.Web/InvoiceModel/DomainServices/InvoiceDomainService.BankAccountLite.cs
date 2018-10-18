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
        public IQueryable<BankAccountLiteList> GetBankAccountLiteLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            BankAccountLiteRepository = new BankAccountLiteRepository(tenant);
            BankAccountLiteQuery = new BankAccountLiteQuery(BankAccountLiteRepository);
            IQueryable<BankAccountLite> iQueryable = BankAccountLiteRepository.GetBankAccountLites(tenant);
            IQueryable<BankAccountLiteList> query2 = BankAccountLiteQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }
        [Query(HasSideEffects = true)]
        public IQueryable<BankAccountLiteList> GetBankAccountLiteFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            BankAccountLiteRepository = new BankAccountLiteRepository(tenant);
            BankAccountLiteQuery = new BankAccountLiteQuery(BankAccountLiteRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<BankAccountLite> iQueryable = BankAccountLiteRepository.GetBankAccountLites(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<BankAccountLite>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<BankAccountLiteList> query2 = BankAccountLiteQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<BankAccountLiteList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(BankAccountLiteList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<BankAccountLiteList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<BankAccountLiteList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<BankAccountLiteList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<BankAccountLiteList, int>(queryOperations, query2);
                            break;
                        }
                    default:
                        {
                            query2 = query2.OrderByDescending(d => d.CreateDate);
                            break;
                        }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.CreateDate);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }
        public int GetBankAccountLiteCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            BankAccountLiteRepository = new BankAccountLiteRepository(tenant);
            BankAccountLiteQuery = new BankAccountLiteQuery(BankAccountLiteRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<BankAccountLite> iQueryable = BankAccountLiteRepository.GetBankAccountLites(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<BankAccountLite>(nonListQueryOperation, iQueryable);

            IQueryable<BankAccountLiteList> query2 = BankAccountLiteQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<BankAccountLiteList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}