using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InvoiceModel.Repositories
{
    public class APPaymentRepository: IRepository<APPayment>
    {
        IInvoiceContext invoiceContext;
        public APPaymentRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public APPaymentRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public APPaymentRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public APPayment GetSingleAPPayment(string id, int tenant)
        {
            return (from a in context.APPayments.Include("LocalCurrency").Include("AccountingPaymentMethod").Include("PaymentCurrency").Include("VendorCard").Include("CreatedByUser.Contact").Include("Status").Include("Branch").Include("TransferStatus")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public APPayment GetSingleAPPayment(string id)
        {
            return (from a in context.APPayments
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        /* Accounting Summary */
        //public int GetDraftsAPPaymentsCount(int tenant)
        //{
        //    return context.APPayments.Where(d => d.Tenant == tenant && d.StatusCode == "DR").Count();
        //}

        public IQueryable<APPayment> GetDraftsAPPayments(int tenant)
        {
            return context.APPayments.Where(d => d.Tenant == tenant && d.StatusCode == "DR");
        }

        public IQueryable<APPayment> GetErrorInTransferAPPayments(int tenant)
        {
            return context.APPayments.Where(d => d.Tenant == tenant && d.TransferStatusCode == "ET");
        }


        public IQueryable<APPayment> GetOpenedAPPayments(int tenant)
        {
            return ( from d in context.APPayments.Include("AccountingPaymentMethod").Include("Status")
                     where d.Tenant == tenant && d.StatusCode != "DR" && d.StatusCode != "VD" && d.StatusCode != "LL" && d.IsClosed == false
                     select d);
        }

        public IQueryable<APPayment> GetAccountingLedgerAPPayments(int tenant)
        {
            return context.APPayments.Where(d => d.Tenant == tenant && d.StatusCode != "DR" && d.StatusCode != "VD" && d.StatusCode != "AC");
        }

        //public int GetOpenedAPPaymentsCount(int tenant)
        //{
        //    return context.APPayments.Where(d => d.Tenant == tenant && d.StatusCode != "DR" && d.StatusCode != "VD" && d.IsClosed == false).Count();
        //}

        public List<APPayment> GetAPPaymentsBySupplier(string supplierId, int tenant)
        {
            return (from a in context.APPayments where a.VendorId == supplierId && a.Tenant == tenant select a).ToList();
        }


        public IQueryable<APPayment> GetAPPayments(int tenant)
        {
            return (from a in context.APPayments
                    where a.Tenant == tenant
                    select a);
        }

     
        public bool IsAPPaymentNumberExists(string apPaymentNo,int tenant)
        {
            return context.APPayments.Where(d => d.PaymentNo == apPaymentNo && d.Tenant == tenant).Any();
        }
      
        public void Add(APPayment entity)
        {
            context.APPayments.Add(entity);
        }

        public void Remove(APPayment entity)
        {
            context.APPayments.Attach(entity);
            context.APPayments.Remove(entity);
        }

        public void Update(APPayment entity)
        {
            try
            {
                context.APPayments.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<APPayment> All()
        {
            return context.APPayments.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<APPayment> GetPaymentsListFromIdList(List<string> ids, int tenant)
        {
            List<APPayment> list = new List<APPayment>();

            if (ids.Count > 0)
            {
                list = (from a in context.APPayments.Include("PaymentCurrency")
                        where a.Tenant == tenant && ids.Contains(a.Id)
                        select a).ToList();
            }

            return list;
        }

        public List<APPayment> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public APPayment GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
