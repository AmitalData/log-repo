using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;

using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;

using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.DataContracts;

namespace WebFreight.Web.InvoiceModel.DomainServices
{
    public partial class InvoiceDomainService
    {
        private ARInvoiceTotalVATRepository aRInvoiceTotalVatRepository;

        public InvoiceTotalsClass GetInvoiceTotalsClass()
        {
            return new InvoiceTotalsClass();
        }

        //public ARInvoiceTotalVATPM GetSingleInvoiceTotalVAT(string id, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("ARInvoiceTotalVAT", "READ", tenant);

        //    arInvoiceTotalVatQuery = new ARInvoiceTotalVATQuery(tenant);
        //    return arInvoiceTotalVatQuery.GetSingleInvoiceTotalVATPM(id);
        //}

        public void UpdateARInvoiceTotalVATList(ARInvoiceTotalVATList list)
        {

        }

        public ARInvoiceTotalVATList GetSingleInvoiceTotalVATList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARInvoiceTotalVAT", "READ", tenant);

            ARInvoiceTotalVATList entityList = null;
            aRInvoiceTotalVatRepository = new ARInvoiceTotalVATRepository(tenant);
            arInvoiceTotalVatQuery = new ARInvoiceTotalVATQuery(aRInvoiceTotalVatRepository);
            ARInvoiceTotalVAT entity = aRInvoiceTotalVatRepository.GetSingleInvoiceTotalVAT(id);

            if (entity != null)
            {
                List<ARInvoiceTotalVAT> SingleEntityList = new List<ARInvoiceTotalVAT>();
                SingleEntityList.Add(entity);

                IQueryable<ARInvoiceTotalVAT> iQueryable = SingleEntityList.AsQueryable();
                IQueryable<ARInvoiceTotalVATList> iQueryableEntityList = arInvoiceTotalVatQuery.GetIQueryableEntityList(iQueryable);
                entityList = iQueryableEntityList.FirstOrDefault();
            }

            return entityList;
        }

        public IQueryable<ARInvoiceTotalVATList> GetInvoiceTotalVATLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARInvoiceTotalVAT", "READ", tenant);

            aRInvoiceTotalVatRepository = new ARInvoiceTotalVATRepository(tenant);
            arInvoiceTotalVatQuery = new ARInvoiceTotalVATQuery(aRInvoiceTotalVatRepository);

            IQueryable<ARInvoiceTotalVAT> iQueryable = aRInvoiceTotalVatRepository.GetInvoiceTotalVATsByTenant(tenant);
            IQueryable<ARInvoiceTotalVATList> query2 = arInvoiceTotalVatQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ARInvoiceTotalVATList> GetInvoiceTotalVATFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARInvoiceTotalVAT", "READ", tenant);

            aRInvoiceTotalVatRepository = new ARInvoiceTotalVATRepository(tenant);
            arInvoiceTotalVatQuery = new ARInvoiceTotalVATQuery(aRInvoiceTotalVatRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARInvoiceTotalVAT> iQueryable = aRInvoiceTotalVatRepository.GetInvoiceTotalVATsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoiceTotalVAT>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ARInvoiceTotalVATList> query2 = arInvoiceTotalVatQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoiceTotalVATList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ARInvoiceTotalVATList).GetProperty(queryOperations.SortByColumnName);
                switch (propInfo.PropertyType.Name.ToLower())
                {
                    case "string":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceTotalVATList, string>(queryOperations, query2);
                            break;
                        }
                    case "double":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceTotalVATList, double>(queryOperations, query2);
                            break;
                        }
                    case "datetime":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceTotalVATList, DateTime>(queryOperations, query2);
                            break;
                        }
                    case "int":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceTotalVATList, int>(queryOperations, query2);
                            break;
                        }
                    case "boolean":
                        {
                            query2 = sortClass.GetSorterQuery<ARInvoiceTotalVATList, bool>(queryOperations, query2);
                            break;
                        }
                    default:
                        {
                            query2 = query2.OrderByDescending(d => d.Id);
                            break;
                        }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.Id);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetInvoiceTotalVATCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ARInvoiceTotalVAT", "READ", tenant);

            aRInvoiceTotalVatRepository = new ARInvoiceTotalVATRepository(tenant);
            arInvoiceTotalVatQuery = new ARInvoiceTotalVATQuery(aRInvoiceTotalVatRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ARInvoiceTotalVAT> iQueryable = aRInvoiceTotalVatRepository.GetInvoiceTotalVATsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ARInvoiceTotalVAT>(nonListQueryOperation, iQueryable);

            IQueryable<ARInvoiceTotalVATList> query2 = arInvoiceTotalVatQuery.GetIQueryableEntityList(iQueryable);

            query2 = filter.GetFilteredQuery<ARInvoiceTotalVATList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void MapInvoiceTotalVATPMInvoiceTotalVAT(ARInvoiceTotalVATPM invoiceTotalVatpm, ARInvoiceTotalVAT invoiceTotalVat)
        {
            invoiceTotalVat.InvoiceCurrencyVatableAmount = invoiceTotalVatpm.InvoiceCurrencyVatableAmount;
            invoiceTotalVat.InvoiceCurrencyVATAmount = invoiceTotalVatpm.InvoiceCurrencyVATAmount;
            invoiceTotalVat.LocalVatableAmount = invoiceTotalVatpm.LocalVatableAmount;
            invoiceTotalVat.LocalVATAmount = invoiceTotalVatpm.LocalVATAmount;
            invoiceTotalVat.Tenant = invoiceTotalVatpm.Tenant;
            invoiceTotalVat.VatPercent = invoiceTotalVatpm.VATPercent;
            invoiceTotalVat.VatTypeId = invoiceTotalVatpm.VatTypeId;
            invoiceTotalVat.ProfitCurrencyVATAmount = invoiceTotalVatpm.ProfitCurrencyVATAmount;
            invoiceTotalVat.ProfitVatableAmount = invoiceTotalVatpm.ProfitVatableAmount;
        }

        public void InsertInvoiceTotalVAT(ARInvoiceTotalVATPM entity)
        {
            SecurityUtility.AuthenticationOnTenant(entity.Tenant);
            SecurityUtility.CheckContactFeature("ARInvoiceTotalVAT", "NEW", entity.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entity.Tenant);
            }

            aRInvoiceTotalVatRepository = new ARInvoiceTotalVATRepository(objectContext);

            entity.Id = IdCounter.GetNumber("ARInvoiceTotalVAT", entity.Tenant).ToString();

            ARInvoiceTotalVAT newInvoiceTotalVat = new ARInvoiceTotalVAT();
            newInvoiceTotalVat.Id = entity.Id;

            MapInvoiceTotalVATPMInvoiceTotalVAT(entity, newInvoiceTotalVat);
            aRInvoiceTotalVatRepository.Add(newInvoiceTotalVat);

        }

        public void UpdateInvoiceTotalVAT(ARInvoiceTotalVATPM currentEntity)
        {
            SecurityUtility.AuthenticationOnTenant(currentEntity.Tenant);
            SecurityUtility.CheckContactFeature("ARInvoiceTotalVAT", "UPDATE", currentEntity.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(currentEntity.Tenant);
            }

            aRInvoiceTotalVatRepository = new ARInvoiceTotalVATRepository(objectContext);
            ARInvoiceTotalVAT invoiceTotalVat = aRInvoiceTotalVatRepository.GetSingleInvoiceTotalVAT(currentEntity.Id);
            MapInvoiceTotalVATPMInvoiceTotalVAT(currentEntity, invoiceTotalVat);
            aRInvoiceTotalVatRepository.Update(invoiceTotalVat);
        }

        public void DeleteInvoiceTotalVAT(ARInvoiceTotalVATPM entity)
        {
            SecurityUtility.AuthenticationOnTenant(entity.Tenant);

            if (objectContext == null)
            {
                objectContext = InvoiceContext.GetContext(entity.Tenant);
            }

            aRInvoiceTotalVatRepository = new ARInvoiceTotalVATRepository(objectContext);
            ARInvoiceTotalVAT invoice = aRInvoiceTotalVatRepository.GetSingleInvoiceTotalVAT(entity.Id);
            aRInvoiceTotalVatRepository.Remove(invoice);
        }
    }
}