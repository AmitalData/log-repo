using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;

using Logitude.BL.InvoiceModel.EntityPMs;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class ARInvoicePaymentQuery
    {
        ARInvoicePaymentRepository repository;
        public ARInvoicePaymentQuery()
        {
            repository = new ARInvoicePaymentRepository(); 
        }
        public ARInvoicePaymentQuery(int tenant)
        {
            repository = new ARInvoicePaymentRepository(tenant);
        }
        public ARInvoicePaymentQuery(ARInvoicePaymentRepository arInvoiceLineRepository)
        {
            repository = arInvoiceLineRepository;
        }

        public List<ARInvoicePaymentPM> GetARInvoicePaymentPMsForInvoice(string invoiceid, int tenant)
        {
            List<ARInvoicePaymentPM> result =
             (from a in repository.context.ARInvoicePayments.Include("ARPayment")
              where a.ARInvoiceId == invoiceid && a.Tenant == tenant
              select new ARInvoicePaymentPM()
              {
                  ARInvoiceId = a.ARInvoiceId,
                  Id = a.Id,
                  ForeignAmount = a.ForeignAmount,
                  ForeignCurrencyId = a.ForeignCurrencyId,
                  LocalAmount = a.LocalAmount,
                  Tenant = a.Tenant,
                  ARPaymentId = a.ARPaymentId,
                  ExchangeRate = a.ExchangeRate,
                  PaymentAmount = a.PaymentAmount,
                  PaymentNumber = a.ARPayment == null ? null : a.ARPayment.PaymentNo,
                  
              }).ToList();


            return result;
        }

        public List<ARPaymentInvoicePM> GetARPaymentInvoicePMsForPayment(string paymentid, int tenant)
        {
            return (from a in repository.context.ARInvoicePayments.Include("ARInvoice")
                    where a.ARPaymentId == paymentid && a.Tenant == tenant
                    select new ARPaymentInvoicePM()
                    {
                        ARInvoiceId = a.ARInvoiceId,
                        Id = a.Id,
                        ForeignAmount = a.ForeignAmount,
                        PaymentAmount = a.PaymentAmount,
                        LocalAmount = a.LocalAmount,
                        ExchangeRate = a.ExchangeRate,
                        ForeignCurrencyId = a.ForeignCurrencyId,
                        Tenant = a.Tenant,
                        ARPaymentId = a.ARPaymentId,
                        ARInvoiceNumber = a.ARInvoice == null ? null : a.ARInvoice.InvoiceNumber,        
                        ARInvoiceMetodoPagoCode = a.ARInvoice == null ? null : a.ARInvoice.MetodoPagoCode,
                        ARInvoiceTransferStatusCode=a.ARInvoice==null?null:a.ARInvoice.TransferStatusCode,
                    }).ToList();
        }

        public IQueryable<ARPaymentInvoicePM> GetPaymentLinesForMessaging(int tenant)
        {
            return (from a in repository.context.ARInvoicePayments.Include("ARInvoice")
                    where a.Tenant == tenant
                    select new ARPaymentInvoicePM()
                    {
                        ARInvoiceId = a.ARInvoiceId,
                        Id = a.Id,
                        ForeignAmount = a.ForeignAmount,
                        ForeignCurrencyId = a.ForeignCurrencyId,
                        LocalAmount = a.LocalAmount,
                        Tenant = a.Tenant,
                        ARPaymentId = a.ARPaymentId,
                        ARInvoiceNumber = a.ARInvoice == null ? null : a.ARInvoice.InvoiceNumber,
                        ExchangeRate = a.ExchangeRate,
                        PaymentAmount = a.PaymentAmount,
                    });
        }


      
    }
}