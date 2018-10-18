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
        public AccountingPaymentMethodPM GetSinglePaymentMethods(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentMethodQuery = new AccountingPaymentMethodQuery(tenant);
            return paymentMethodQuery.GetSinglePaymentMethodPM(code, tenant);
        }

        public void UpdatePaymentMethodList(AccountingPaymentMethodList list)
        {

        }

        public AccountingPaymentMethodList GetSinglePaymentMethodList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentMethodRepository = new AccountingPaymentMethodRepository(tenant);
            paymentMethodQuery = new AccountingPaymentMethodQuery(paymentMethodRepository);
            AccountingPaymentMethodList entityList = null;
            AccountingPaymentMethod entity = paymentMethodRepository.GetSingleAccountingPaymentMethod(code, tenant);

            if (entity != null)
            {
                List<AccountingPaymentMethod> singleEntityList = new List<AccountingPaymentMethod>();
                singleEntityList.Add(entity);

                IQueryable<AccountingPaymentMethod> iQueryable = singleEntityList.AsQueryable();
                IQueryable<AccountingPaymentMethodList> iQueryableEntityList = paymentMethodQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<AccountingPaymentMethodList> GetPaymentMethodLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentMethodRepository = new AccountingPaymentMethodRepository(tenant);
            paymentMethodQuery = new AccountingPaymentMethodQuery(paymentMethodRepository);
            IQueryable<AccountingPaymentMethod> iQueryable = paymentMethodRepository.GetAccountingPaymentMethods(tenant);
            IQueryable<AccountingPaymentMethodList> query2 = paymentMethodQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<AccountingPaymentMethodList> GetPaymentMethodFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentMethodRepository = new AccountingPaymentMethodRepository(tenant);
            paymentMethodQuery = new AccountingPaymentMethodQuery(paymentMethodRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AccountingPaymentMethod> iQueryable = paymentMethodRepository.GetAccountingPaymentMethods(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AccountingPaymentMethod>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<AccountingPaymentMethodList> query2 = paymentMethodQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<AccountingPaymentMethodList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(AccountingPaymentMethodList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<AccountingPaymentMethodList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<AccountingPaymentMethodList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<AccountingPaymentMethodList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<AccountingPaymentMethodList, int>(queryOperations, query2);
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

        public int GetPaymentMethodCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            paymentMethodRepository = new AccountingPaymentMethodRepository(tenant);
            paymentMethodQuery = new AccountingPaymentMethodQuery(paymentMethodRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<AccountingPaymentMethod> iQueryable = paymentMethodRepository.GetAccountingPaymentMethods(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<AccountingPaymentMethod>(nonListQueryOperation, iQueryable);

            IQueryable<AccountingPaymentMethodList> query2 = paymentMethodQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<AccountingPaymentMethodList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}