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
        public SATPaymentMethodPM GetSingleSATPaymentMethods(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            SATPaymentMethodQuery = new SATPaymentMethodQuery(tenant);
            return SATPaymentMethodQuery.GetSingleSATPaymentMethodPM(code);
        }

        public void UpdateSATPaymentMethodList(SATPaymentMethodList list)
        {

        }

        public SATPaymentMethodList GetSingleSATPaymentMethodList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            SATPaymentMethodRepository = new SATPaymentMethodRepository(tenant);
            SATPaymentMethodQuery = new SATPaymentMethodQuery(SATPaymentMethodRepository);
            SATPaymentMethodList entityList = null;
            SATPaymentMethod entity = SATPaymentMethodRepository.GetSingleSATPaymentMethod(code);

            if (entity != null)
            {
                List<SATPaymentMethod> singleEntityList = new List<SATPaymentMethod>();
                singleEntityList.Add(entity);

                IQueryable<SATPaymentMethod> iQueryable = singleEntityList.AsQueryable();
                IQueryable<SATPaymentMethodList> iQueryableEntityList = SATPaymentMethodQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<SATPaymentMethodList> GetSATPaymentMethodLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            SATPaymentMethodRepository = new SATPaymentMethodRepository(tenant);
            SATPaymentMethodQuery = new SATPaymentMethodQuery(SATPaymentMethodRepository);
            IQueryable<SATPaymentMethod> iQueryable = SATPaymentMethodRepository.GetSATPaymentMethods();
            IQueryable<SATPaymentMethodList> query2 = SATPaymentMethodQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<SATPaymentMethodList> GetSATPaymentMethodFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            SATPaymentMethodRepository = new SATPaymentMethodRepository(tenant);
            SATPaymentMethodQuery = new SATPaymentMethodQuery(SATPaymentMethodRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SATPaymentMethod> iQueryable = SATPaymentMethodRepository.GetSATPaymentMethods();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SATPaymentMethod>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<SATPaymentMethodList> query2 = SATPaymentMethodQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<SATPaymentMethodList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(SATPaymentMethodList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<SATPaymentMethodList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<SATPaymentMethodList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<SATPaymentMethodList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<SATPaymentMethodList, int>(queryOperations, query2);
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

        public int GetSATPaymentMethodCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            SATPaymentMethodRepository = new SATPaymentMethodRepository(tenant);
            SATPaymentMethodQuery = new SATPaymentMethodQuery(SATPaymentMethodRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<SATPaymentMethod> iQueryable = SATPaymentMethodRepository.GetSATPaymentMethods();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<SATPaymentMethod>(nonListQueryOperation, iQueryable);

            IQueryable<SATPaymentMethodList> query2 = SATPaymentMethodQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<SATPaymentMethodList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }
    }
}