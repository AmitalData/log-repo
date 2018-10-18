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
        public APInvoiceTypePM GetSingleAPInvoiceType(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            apInvoiceTypeQuery = new APInvoiceTypeQuery(aPInvoiceTypeRepository);           
            return apInvoiceTypeQuery.GetSingleAPInvoiceTypePM(code);
        }

        public  void UpdateAPInvoiceTypeList(APInvoiceTypeList list)
        {

        }

        public APInvoiceTypeList GetSingleAPInvoiceTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            APInvoiceTypeList entityList = null;
            aPInvoiceTypeRepository = new APInvoiceTypeRepository(tenant);
            apInvoiceTypeQuery = new APInvoiceTypeQuery(aPInvoiceTypeRepository);
            APInvoiceType entity = aPInvoiceTypeRepository.GetSingleAPInvoiceType(code);
            
            if (entity != null)
            {
                List<APInvoiceType> SingleEntityList = new List<APInvoiceType>();
                SingleEntityList.Add(entity);

                IQueryable<APInvoiceType> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<APInvoiceTypeList> iQueryableEntityList = apInvoiceTypeQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<APInvoiceTypeList> GetAPInvoiceTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            aPInvoiceTypeRepository = new APInvoiceTypeRepository(tenant);
            apInvoiceTypeQuery = new APInvoiceTypeQuery(aPInvoiceTypeRepository);
            IQueryable<APInvoiceType> iQueryable = aPInvoiceTypeRepository.GetAPInvoiceTypes();
            IQueryable<APInvoiceTypeList> query2 = apInvoiceTypeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<APInvoiceTypeList> GetAPInvoiceTypeFilters(byte[] XmlFilters, int tenant)
        {
            aPInvoiceTypeRepository = new APInvoiceTypeRepository(tenant);
            apInvoiceTypeQuery = new APInvoiceTypeQuery(aPInvoiceTypeRepository);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<APInvoiceType> iQueryable = aPInvoiceTypeRepository.GetAPInvoiceTypes();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APInvoiceType>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<APInvoiceTypeList> query2 = apInvoiceTypeQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<APInvoiceTypeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(APInvoiceTypeList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<APInvoiceTypeList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<APInvoiceTypeList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<APInvoiceTypeList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<APInvoiceTypeList, int>(queryOperations, query2);
                            break;
                        }
                    case "boolean":
                        {
                            query2 = sortClass.GetSorterQuery<APInvoiceTypeList, bool>(queryOperations, query2);
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

        public int GetAPInvoiceTypeCount(byte[] XmlFilters, int tenant)
        {
            aPInvoiceTypeRepository = new APInvoiceTypeRepository(tenant);
            apInvoiceTypeQuery = new APInvoiceTypeQuery(aPInvoiceTypeRepository);
            SecurityUtility.AuthenticationOnTenant(tenant);
            MemoryStream memorystream = new MemoryStream(XmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<APInvoiceType> iQueryable = aPInvoiceTypeRepository.GetAPInvoiceTypes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<APInvoiceType>(nonListQueryOperation, iQueryable);

            IQueryable<APInvoiceTypeList> query2 = apInvoiceTypeQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<APInvoiceTypeList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }
    }
}