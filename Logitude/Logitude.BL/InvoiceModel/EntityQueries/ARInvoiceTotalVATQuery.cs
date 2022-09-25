using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ARInvoiceTotalVATQuery
    {
        private ARInvoiceTotalVATRepository repository;
        
        public ARInvoiceTotalVATQuery()
        {
            repository = new ARInvoiceTotalVATRepository(); 
        }

        public ARInvoiceTotalVATQuery(int tenant)
        {
            repository = new ARInvoiceTotalVATRepository(tenant);
        }

        public ARInvoiceTotalVATQuery(ARInvoiceTotalVATRepository arInvoiceTotalVATRepository)
        {
            repository = arInvoiceTotalVATRepository;
        }

        public IQueryable<ARInvoiceTotalVATPM> GetTotalVATs(string invoiceId, int tenant)
        {
            return from a in repository.context
                                       .ARInvoiceTotalVATs
                                       .Include("VatType")
                   where a.Tenant == tenant && a.ARInvoiceId == invoiceId
                   select new ARInvoiceTotalVATPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       ARInvoiceId = a.ARInvoiceId,
                       ExternalVATCard = a.ExternalVATCard,
                       ExternalTAXItemId = a.ExternalTAXItemId,
                       InvoiceCurrencyVatableAmount = a.InvoiceCurrencyVatableAmount,
                       InvoiceCurrencyVATAmount = a.InvoiceCurrencyVATAmount,
                       LocalVatableAmount = a.LocalVatableAmount,
                       LocalVATAmount = a.LocalVATAmount,
                       ProfitCurrencyVATAmount = a.ProfitCurrencyVATAmount,
                       ProfitVatableAmount = a.ProfitVatableAmount,
                       VatTypeId = a.VatTypeId,
                       VATPercent = a.VatPercent,
                       VatTypeName = a.VatType == null ? null : a.VatType.EnglishName,
                       VatTypeCell = a.VatType == null ? null : (a.VatType.EnglishName + " (" + a.VatPercent + "%)"),
                       VatTypeCode = a.VatType == null ? null : a.VatType.Code,
                       IsRegionalTax = a.IsRegionalTax,
                   };
        }

        public IQueryable<ARInvoiceTotalVATPM> GetTotalVATsByInvoicesIds(List<string> invoicesIds, int tenant)
        {
            return repository.context
                             .ARInvoiceTotalVATs
                             .Include("VatType")
                   .Where(a => a.Tenant == tenant && invoicesIds.Contains(a.ARInvoiceId)) 
                   .Select(a => new ARInvoiceTotalVATPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       ARInvoiceId = a.ARInvoiceId,
                       ExternalVATCard = a.ExternalVATCard,
                       ExternalTAXItemId = a.ExternalTAXItemId,
                       InvoiceCurrencyVatableAmount = a.InvoiceCurrencyVatableAmount,
                       InvoiceCurrencyVATAmount = a.InvoiceCurrencyVATAmount,
                       LocalVatableAmount = a.LocalVatableAmount,
                       LocalVATAmount = a.LocalVATAmount,
                       ProfitCurrencyVATAmount = a.ProfitCurrencyVATAmount,
                       ProfitVatableAmount = a.ProfitVatableAmount,
                       VatTypeId = a.VatTypeId,
                       VATPercent = a.VatPercent,
                       VatTypeName = a.VatType == null ? null : a.VatType.EnglishName,
                       VatTypeCell = a.VatType == null ? null : (a.VatType.EnglishName + " (" + a.VatPercent + "%)"),
                       VatTypeCode = a.VatType == null ? null : a.VatType.Code,
                       IsRegionalTax = a.IsRegionalTax,
                   });
        }

        public IQueryable<ARInvoiceTotalVATList> GetIQueryableEntityList(IQueryable<ARInvoiceTotalVAT> iQueryable)
        {
            IQueryable<ARInvoiceTotalVATList> result = from entity in iQueryable
                                                       select new ARInvoiceTotalVATList()
                                                       {
                                                           Id = entity.Id,
                                                           InvoiceCurrencyVatableAmount = entity.InvoiceCurrencyVatableAmount,
                                                           InvoiceCurrencyVATAmount = entity.InvoiceCurrencyVATAmount,
                                                           LocalVatableAmount = entity.LocalVatableAmount,
                                                           LocalVATAmount = entity.LocalVATAmount,
                                                           Tenant = entity.Tenant,
                                                           VATPercent = entity.VatPercent,
                                                           VatTypeId = entity.VatTypeId,
                                                           ARInvoiceId = entity.ARInvoiceId,
                                                           IsRegionalTax = entity.IsRegionalTax,
                                                       };
            return result;
        }

        public List<ARInvoiceTotalVATPM> GetTotalVATs(List<string> invoiceIds, int tenant)

        {
            List<ARInvoiceTotalVATPM> list = new List<ARInvoiceTotalVATPM>();
            const int sqlLimit = 5000;

            int iterations = invoiceIds.Count() / sqlLimit;
            for (int i = 0; i <= iterations; i++)
            {
                var tempInvoiceIds = invoiceIds.Skip(i * sqlLimit).Take(sqlLimit).ToList();
                List<ARInvoiceTotalVATPM> tempList = (from a in repository.context.ARInvoiceTotalVATs
                                                      where a.Tenant == tenant && tempInvoiceIds.Contains(a.ARInvoiceId)
                                                      select new ARInvoiceTotalVATPM()
                                                      {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          ARInvoiceId = a.ARInvoiceId,
                                                          ExternalVATCard = a.ExternalVATCard,
                                                          ExternalTAXItemId = a.ExternalTAXItemId,
                                                          InvoiceCurrencyVatableAmount = a.InvoiceCurrencyVatableAmount,
                                                          InvoiceCurrencyVATAmount = a.InvoiceCurrencyVATAmount,
                                                          LocalVatableAmount = a.LocalVatableAmount,
                                                          LocalVATAmount = a.LocalVATAmount,
                                                          ProfitCurrencyVATAmount = a.ProfitCurrencyVATAmount,
                                                          ProfitVatableAmount = a.ProfitVatableAmount,
                                                          VatTypeId = a.VatTypeId,
                                                          VATPercent = a.VatPercent,
                                                          VatTypeName = a.VatType == null ? null : a.VatType.EnglishName,
                                                          VatTypeCell = a.VatType == null ? null : (a.VatType.EnglishName + " (" + a.VatPercent + "%)"),
                                                          IsRegionalTax = a.IsRegionalTax,
                                                      }).ToList();
                list.AddRange(tempList);
            }
            return list;
        }


    }
}