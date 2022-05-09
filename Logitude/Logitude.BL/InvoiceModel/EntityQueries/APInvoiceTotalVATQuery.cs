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
    public class APInvoiceTotalVATQuery
    {
        APInvoiceTotalVATRepository repository;
        public APInvoiceTotalVATQuery()
        {
            repository = new APInvoiceTotalVATRepository(); 
        }

        public APInvoiceTotalVATQuery(int tenant)
        {
            repository = new APInvoiceTotalVATRepository(tenant);
        }

        public APInvoiceTotalVATQuery(APInvoiceTotalVATRepository apInvoiceTotalVATRepository)
        {
            repository = apInvoiceTotalVATRepository;
        }

        public APInvoiceTotalVATPM GetSinglePM(string id, int tenant)
        {
            return (from a in repository.context.APInvoiceTotalVATs.Include("VatType")
                   where a.Tenant == tenant && a.Id == id
                   select new APInvoiceTotalVATPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       APInvoiceId = a.APInvoiceId,
                       ExternalVATCard = a.ExternalVATCard,
                       ExternalTAXItemId = a.ExternalTAXItemId,
                       InvoiceCurrencyVatableAmount = a.InvoiceCurrencyVatableAmount,
                       InvoiceCurrencyVATAmount = a.InvoiceCurrencyVATAmount,
                       LocalVatableAmount = a.LocalVatableAmount,
                       LocalVATAmount = a.LocalVATAmount,
                       ProfitCurrencyVATAmount = a.ProfitCurrencyVATAmount,
                       ProfitVatableAmount = a.ProfitVatableAmount,
                       VatTypeId = a.VatTypeId,
                       VatPercent = a.VatPercent,
                       VatTypeName = a.VatType == null ? null : a.VatType.EnglishName,
                       VatTypeCell = a.VatType == null ? null : (a.VatType.EnglishName + " (" + a.VatPercent + "%)"),
                   }).FirstOrDefault();
        }

        public IQueryable<APInvoiceTotalVATPM> GetTotalVATs(string invoiceId, int tenant)
        {
            return from a in repository.context.APInvoiceTotalVATs.Include("VatType")
                   where a.Tenant == tenant && a.APInvoiceId == invoiceId
                   select new APInvoiceTotalVATPM()
                   {
                       Id = a.Id,
                       Tenant = a.Tenant,
                       APInvoiceId = a.APInvoiceId,
                       ExternalVATCard = a.ExternalVATCard,
                       ExternalTAXItemId = a.ExternalTAXItemId,
                       InvoiceCurrencyVatableAmount = a.InvoiceCurrencyVatableAmount,
                       InvoiceCurrencyVATAmount = a.InvoiceCurrencyVATAmount,
                       LocalVatableAmount = a.LocalVatableAmount,
                       LocalVATAmount = a.LocalVATAmount,
                       ProfitCurrencyVATAmount = a.ProfitCurrencyVATAmount,
                       ProfitVatableAmount = a.ProfitVatableAmount,
                       VatTypeId = a.VatTypeId,
                       VatPercent = a.VatPercent,
                       VatTypeName = a.VatType == null ? null : a.VatType.EnglishName,
                       VatTypeCell = a.VatType == null ? null : (a.VatType.EnglishName + " (" + a.VatPercent + "%)"),                       
                   };
        }

        public IQueryable<APInvoiceTotalVATList> GetIQueryableEntityList(IQueryable<APInvoiceTotalVAT> iQueryable)
        {
            IQueryable<APInvoiceTotalVATList> result = from entity in iQueryable
                                                       select new APInvoiceTotalVATList()
                                                       {
                                                           Id = entity.Id,
                                                           InvoiceCurrencyVatableAmount = entity.InvoiceCurrencyVatableAmount,
                                                           InvoiceCurrencyVATAmount = entity.InvoiceCurrencyVATAmount,
                                                           LocalVatableAmount = entity.LocalVatableAmount,
                                                           LocalVATAmount = entity.LocalVATAmount,
                                                           Tenant = entity.Tenant,
                                                           APInvoiceId = entity.APInvoiceId,
                                                           VatTypeId = entity.VatTypeId,
                                                           VatPercent = entity.VatPercent,

                                                       };
            return result;
        }


        public List<APInvoiceTotalVATPM> GetTotalVATs(List<string> invoiceIds, int tenant)

        {
            List<APInvoiceTotalVATPM> list = new List<APInvoiceTotalVATPM>();
            const int sqlLimit = 5000;

            int iterations = invoiceIds.Count() / sqlLimit;
            for (int i = 0; i <= iterations; i++)
            {
                var tempInvoiceIds = invoiceIds.Skip(i * sqlLimit).Take(sqlLimit).ToList();
                List<APInvoiceTotalVATPM> tempList = (from a in repository.context.APInvoiceTotalVATs
                                                      where a.Tenant == tenant && tempInvoiceIds.Contains(a.APInvoiceId)
                                                      select new APInvoiceTotalVATPM()
                                                      {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          APInvoiceId = a.APInvoiceId,
                                                          ExternalVATCard = a.ExternalVATCard,
                                                          ExternalTAXItemId = a.ExternalTAXItemId,
                                                          InvoiceCurrencyVatableAmount = a.InvoiceCurrencyVatableAmount,
                                                          InvoiceCurrencyVATAmount = a.InvoiceCurrencyVATAmount,
                                                          LocalVatableAmount = a.LocalVatableAmount,
                                                          LocalVATAmount = a.LocalVATAmount,
                                                          ProfitCurrencyVATAmount = a.ProfitCurrencyVATAmount,
                                                          ProfitVatableAmount = a.ProfitVatableAmount,
                                                          VatTypeId = a.VatTypeId,

                                                          VatTypeName = a.VatType == null ? null : a.VatType.EnglishName,
                                                          VatTypeCell = a.VatType == null ? null : (a.VatType.EnglishName + " (" + a.VatPercent + "%)"),
                                                      }).ToList();
                list.AddRange(tempList);
            }
            return list;
        }

        public List<APInvoiceTotalVATPM> GetInvoiceTotalVatsForInvoiceWithoutZeroVATPercent(string invoiceId, int tenant)
        {
           List<APInvoiceTotalVAT> totalVats= repository.GetInvoiceTotalVatsForInvoiceWithoutZeroVATPercent(invoiceId, tenant);

            return (from a in totalVats
                    select new APInvoiceTotalVATPM()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        APInvoiceId = a.APInvoiceId,
                        ExternalVATCard = a.ExternalVATCard,
                        ExternalTAXItemId = a.ExternalTAXItemId,
                        InvoiceCurrencyVatableAmount = a.InvoiceCurrencyVatableAmount,
                        InvoiceCurrencyVATAmount = a.InvoiceCurrencyVATAmount,
                        LocalVatableAmount = a.LocalVatableAmount,
                        LocalVATAmount = a.LocalVATAmount,
                        ProfitCurrencyVATAmount = a.ProfitCurrencyVATAmount,
                        ProfitVatableAmount = a.ProfitVatableAmount,
                        VatTypeId = a.VatTypeId,
                        VatRecognizedPercentage = a.VatType == null ? null : a.VatType.RecognizedPercentage,
                        VatTypeName = a.VatType == null ? null : a.VatType.EnglishName,
                        VatTypeCell = a.VatType == null ? null : (a.VatType.EnglishName + " (" + a.VatPercent + "%)"),
                    }).ToList();
        }


    }
}