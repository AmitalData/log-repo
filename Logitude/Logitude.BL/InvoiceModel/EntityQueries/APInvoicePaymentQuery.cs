using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;

using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class APInvoicePaymentQuery
    {
        APInvoicePaymentRepository repository;
        public APInvoicePaymentQuery()
        {
            repository = new APInvoicePaymentRepository(); 
        }
        public APInvoicePaymentQuery(int tenant)
        {
            repository = new APInvoicePaymentRepository(tenant);
        }
        public APInvoicePaymentQuery(APInvoicePaymentRepository apInvoicePaymentRepository)
        {
            repository = apInvoicePaymentRepository;
        }

        public List<APPaymentInvoicePM> GetAPPaymentInvoicePMsForPayment(string paymentid, int tenant)
        {
            return (from a in repository.context.APInvoicePayments.Include("APInvoice")
                    where a.APPaymentId == paymentid && a.Tenant == tenant
                    select new APPaymentInvoicePM()
                    {
                        APInvoiceId = a.APInvoiceId,
                        Id = a.Id,
                        ForeignAmount = a.ForeignAmount,
                        ForeignCurrencyId = a.ForeignCurrencyId,
                        LocalAmount = a.LocalAmount,
                        Tenant = a.Tenant,
                        APPaymentId = a.APPaymentId,
                        APInvoiceNumber = a.APInvoice.InvoiceNumber,
                        ExchangeRate = a.ExchangeRate,
                        PaymentAmount = a.PaymentAmount,
                    }).ToList();
        }
        public List<APInvoicePaymentPM> GetAPInvoicePaymentPMsForInvoice(string invoiceid, int tenant)
        {
            List<APInvoicePaymentPM> result =
             (from a in repository.context.APInvoicePayments.Include("APPayment")
              where a.APInvoiceId == invoiceid && a.Tenant == tenant
              select new APInvoicePaymentPM()
              {
                  APInvoiceId = a.APInvoiceId,
                  Id = a.Id,
                  ForeignAmount = a.ForeignAmount,
                  ForeignCurrencyId = a.ForeignCurrencyId,
                  LocalAmount = a.LocalAmount,
                  Tenant = a.Tenant,
                  APPaymentId = a.APPaymentId,
                   ExchangeRate = a.ExchangeRate,
                    PaymentAmount = a.PaymentAmount,
                  PaymentNumber = a.APPayment == null ? null : a.APPayment.PaymentNo,
              }).ToList();



            return result;
        }
    }
}