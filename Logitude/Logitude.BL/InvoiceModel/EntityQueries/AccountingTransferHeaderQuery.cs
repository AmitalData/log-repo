using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityLists;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class AccountingTransferHeaderQuery
    {
        AccountingTransferHeaderRepository repository;
        public AccountingTransferHeaderQuery()
        {
            this.repository = new AccountingTransferHeaderRepository();
        }
                
        public AccountingTransferHeaderQuery(int tenant)
        {
            this.repository = new AccountingTransferHeaderRepository(tenant);
        }

        public AccountingTransferHeaderQuery(AccountingTransferHeaderRepository repository)
        {
            this.repository = repository;
        }

        public AccountingTransferHeaderPM GetSinglePM(string id, int tenant)
        {
            AccountingTransferLineQuery linesQuery = new AccountingTransferLineQuery(tenant);

            AccountingTransferHeaderPM entityPM =

                (from a in repository.context.AccountingTransferHeaders.Include("User").Include("User.Contact").Include("TransferType")
                 where a.Id == id && a.Tenant == tenant
                 select new AccountingTransferHeaderPM()
                 {
                     Id = a.Id,
                     AccountingTransferTypeCode = a.AccountingTransferTypeCode,
                     FileName = a.FileName,
                     Tenant = a.Tenant,
                     TransferDate = a.TransferDate,
                     TransferNumber = a.TransferNumber,
                     UserId = a.UserId,
                     UserName = a.User == null ? "" : (a.User.Contact == null ? "" : a.User.Contact.EnglishName),
                     AccountingTransferTypeName = a.TransferType == null ? "" : a.TransferType.Name,
                     Notes = a.Notes,
                 }).FirstOrDefault();

            entityPM.TransferLines = linesQuery.GetAccountingTransferLinePMsForTransferHeader(id, tenant).ToList();

            if (entityPM.AccountingTransferTypeCode == "ARIN")
            {
                ARInvoiceRepository myRepository = new ARInvoiceRepository(tenant);

                foreach (AccountingTransferLinePM item in entityPM.TransferLines)
                {
                    ARInvoice entity = myRepository.GetSingleInvoice(item.EntityId);

                    if (entity != null)
                    {
                        item.InvoiceDate = entity.InvoiceDate;
                        item.AmountInInvoiceCurrency = entity.AmountInInvoiceCurrency;
                        item.BillToName = entity.BillTo == null ? "" : entity.BillTo.EnglishName;
                        item.StatusName = entity.Status == null ? "" : entity.Status.Name;
                        item.InvoiceCurrencyCode = entity.InvoiceCurrency == null ? "" : entity.InvoiceCurrency.Code;
                    }
                }
            }

            else if (entityPM.AccountingTransferTypeCode == "APIN")
            {
                APInvoiceRepository myRepository = new APInvoiceRepository(tenant);

                foreach (AccountingTransferLinePM item in entityPM.TransferLines)
                {
                    APInvoice entity = myRepository.GetSingleAPInvoice(item.EntityId, tenant);

                    if (entity != null)
                    {
                        item.InvoiceDate = entity.InvoiceDate;
                        item.AmountInInvoiceCurrency = entity.AmountInInvoiceCurrency;
                        item.BillToName = entity.VendorCard == null ? "" : entity.VendorCard.EnglishName;
                        item.StatusName = entity.Status == null ? "" : entity.Status.Name;
                        item.InvoiceCurrencyCode = entity.InvoiceCurrency == null ? "" : entity.InvoiceCurrency.Code;
                    }
                }
            }

            else if (entityPM.AccountingTransferTypeCode == "ARPA")
            {
                ARPaymentRepository myRepository = new ARPaymentRepository(tenant);

                foreach (AccountingTransferLinePM item in entityPM.TransferLines)
                {
                    ARPayment entity = myRepository.GetSingleARPayment(item.EntityId, tenant);

                    if (entity != null)
                    {
                        item.InvoiceDate = entity.RegisterDate;
                        item.AmountInInvoiceCurrency = entity.AmountInPaymentCurrency;
                        item.BillToName = entity.BillToCard == null ? "" : entity.BillToCard.EnglishName;
                        item.StatusName = entity.Status == null ? "" : entity.Status.Name;
                        item.InvoiceCurrencyCode = entity.PaymentCurrency == null ? "" : entity.PaymentCurrency.Code;
                    }
                }
            }

            return entityPM;
        }

        public IQueryable<AccountingTransferHeaderList> GetIQueryableEntityList(IQueryable<AccountingTransferHeader> iQueryable)
        {
            var result = from a in iQueryable
                         select new AccountingTransferHeaderList()
                         {
                             Id = a.Id,
                             AccountingTransferTypeCode = a.AccountingTransferTypeCode,
                             FileName = a.FileName,
                             SearchFields = a.SearchFields,
                             Tenant = a.Tenant,
                             TransferDate = a.TransferDate,
                             TransferNumber = a.TransferNumber,
                             UserId = a.UserId,
                             Notes = a.Notes,
                         };

            return result;
        }
    }
}