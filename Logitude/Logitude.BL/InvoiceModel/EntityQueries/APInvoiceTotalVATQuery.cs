using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using Intuit.Ipp.Data;
using CWXSD;

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

            List<APInvoiceTotalVATPM> rv;

            List<APInvoiceTotalVATPM> interimList =  (from a in totalVats
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
            if (interimList != null & interimList.Count > 1)
            {
                rv = interimList.GroupBy(i => i.APInvoiceId)
                .Select(i =>  new APInvoiceTotalVATPM()
                {
                    Id = i.First().Id,
                    Tenant = i.First().Tenant,
                    APInvoiceId = i.First().APInvoiceId,
                    ExternalVATCard = i.First().ExternalVATCard,
                    ExternalTAXItemId = i.First().ExternalTAXItemId,
                    InvoiceCurrencyVatableAmount = i.Sum(item => item.InvoiceCurrencyVatableAmount),
                                            InvoiceCurrencyVATAmount = i.Sum(item => item.InvoiceCurrencyVATAmount),
                                            LocalVatableAmount = i.Sum(item => item.LocalVatableAmount),
                                            LocalVATAmount = i.Sum(item => item.LocalVATAmount),
                                            ProfitCurrencyVATAmount = i.Sum(item => item.ProfitCurrencyVATAmount),
                                            ProfitVatableAmount = i.Sum(item => item.ProfitVatableAmount),
                                            VatTypeId = i.Any(item => item.VatTypeId != null)?  i.First(item => item.VatTypeId != null).VatTypeId : null,
                                            VatRecognizedPercentage = i.Any(item => item.VatTypeId != null) ? i.First(item => item.VatTypeId != null).VatRecognizedPercentage : null,
                                            VatTypeName = i.Any(item => item.VatTypeId != null) ? i.First(item => item.VatTypeId != null).VatTypeName : null,
                                            VatTypeCell = i.Any(item => item.VatTypeId != null) ? i.First(item => item.VatTypeId != null).VatTypeCell : null,
                                            LocalVatAmountWithVatRecognized = i.Sum(item => Math.Round((item.VatRecognizedPercentage != null) ? (((decimal)item.VatRecognizedPercentage / 100) * (decimal)item.LocalVATAmount) : (decimal)item.LocalVATAmount, 2)),

                }).ToList();
            }
            else if (interimList != null)
            {
                APInvoiceTotalVATPM item = interimList[0];
                interimList[0].LocalVatAmountWithVatRecognized = Math.Round((item.VatRecognizedPercentage != null) ? (((decimal)item.VatRecognizedPercentage / 100) * (decimal)item.LocalVATAmount) : (decimal)item.LocalVATAmount, 2);
                rv = interimList;
            }
            else
            {
                rv = new List<APInvoiceTotalVATPM>();
            }
            return rv;
        }




    }
}