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
        public APPaymentMethodPM GetSingleAPPaymentMethods(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            apPaymentMethodQuery = new APPaymentMethodQuery(tenant);
            return apPaymentMethodQuery.GetSingleAPPaymentMethodPM(code, tenant);
        }

        public void UpdateAPPaymentMethodList(APPaymentMethodList list)
        {

        }

        public APPaymentMethodList GetSingleAPPaymentMethodList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            APPaymentMethodList entityList = null;
            aPPaymentMethodRepository = new APPaymentMethodRepository(tenant);
            apPaymentMethodQuery = new APPaymentMethodQuery(aPPaymentMethodRepository);
            APPaymentMethod entity = aPPaymentMethodRepository.GetSingleAPPaymentMethod(code, tenant);

            if (entity != null)
            {
                List<APPaymentMethod> singleEntityList = new List<APPaymentMethod>();
                singleEntityList.Add(entity);

                IQueryable<APPaymentMethod> iQueryable = singleEntityList.AsQueryable();
                IQueryable<APPaymentMethodList> iQueryableEntityList = apPaymentMethodQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<APPaymentMethodList> GetAPPaymentMethodLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPPaymentMethodRepository = new APPaymentMethodRepository(tenant);
            apPaymentMethodQuery = new APPaymentMethodQuery(aPPaymentMethodRepository);

            IQueryable<APPaymentMethod> iQueryable = aPPaymentMethodRepository.GetAPPaymentMethods(tenant);

            IQueryable<APPaymentMethodList> query2 = apPaymentMethodQuery.GetIQueryableEntityList(iQueryable);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<APPaymentMethodList> GetAPPaymentMethodFilters(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPPaymentMethodRepository = new APPaymentMethodRepository(tenant);
            apPaymentMethodQuery = new APPaymentMethodQuery(aPPaymentMethodRepository);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<APPaymentMethod> iQueryable = aPPaymentMethodRepository.GetAPPaymentMethods(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APPaymentMethod>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<APPaymentMethodList> query2 = apPaymentMethodQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<APPaymentMethodList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(APPaymentMethodList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<APPaymentMethodList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<APPaymentMethodList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<APPaymentMethodList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<APPaymentMethodList, int>(queryOperations, query2);
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

        public int GetAPPaymentMethodCount(byte[] XmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPPaymentMethodRepository = new APPaymentMethodRepository(tenant);
            apPaymentMethodQuery = new APPaymentMethodQuery(aPPaymentMethodRepository);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<APPaymentMethod> iQueryable = aPPaymentMethodRepository.GetAPPaymentMethods(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APPaymentMethod>(nonListQueryOperation, iQueryable);

            IQueryable<APPaymentMethodList> query2 = apPaymentMethodQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<APPaymentMethodList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}