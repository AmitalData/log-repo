using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.InvoiceModel.Repositories
{
    public class ARPaymentRepository: IRepository<ARPayment>
    {
        IInvoiceContext invoiceContext;
        ICommonDataContext commonContext;
        public ARPaymentRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public ARPaymentRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARPaymentRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
            commonContext = CommonDataContext.GetContext(tenant);
        }

        public ARPayment GetSingleARPayment(string id, int tenant)
        {
            return (from a in context.ARPayments.Include("ARAccount").Include("AccountingPaymentMethod").Include("BillToCard").Include("CreatedByUser.Contact").Include("DebitAccount").Include("LocalCurrency").Include("PaymentCurrency").Include("Status").Include("SATTransferStatus").Include("TransferStatus").Include("Branch").Include("BankAccountLite")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public ARPayment GetSingleARPayment(string id)
        {
            return (from a in context.ARPayments.Include("PaymentCurrency").Include("BankAccountLite")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public ARPayment GetSingleNotCancelledARPayment(string id, int tenant)
        {
            return (from a in context.ARPayments
                    where a.Id == id && a.Tenant == tenant && a.StatusCode !="VD"
                    select a).FirstOrDefault();
        }

        public List<ARPayment> GetNotCancelledARPayments(List<string> ids, int tenant)
        {
            return (from a in context.ARPayments
                    where ids.Contains(a.Id) && a.Tenant == tenant && a.StatusCode != "VD"
                    select a).ToList();
        }

        public List<string> GetCardIdsFromPayments(List<string> ids, int tenant)
        {
            List<string> list = new List<string>();

            if (ids.Count > 0)
            {
                list = (from a in context.ARPayments
                        where a.Tenant == tenant && ids.Contains(a.Id) && a.StatusCode != "VD"
                        select a.BillToId).ToList();
            }

            return list;
        }

        public User getUserByARPayment(ARPayment aRPayment)
        {
            User createByUser = (from user in commonContext.Users.Include("Contact")
                                   where user.Id == aRPayment.CreatedByUserId
                                   select user).FirstOrDefault();
            return createByUser;
        }

        public IQueryable<ARPayment> GetDraftsARPayments(int tenant)
        {
            return context.ARPayments.Where(d => d.Tenant == tenant && d.StatusCode == "DR");
        }

        public IQueryable<ARPayment> GetOpenedARPayments(int tenant)
        {
            return (from d in context.ARPayments.Include("AccountingPaymentMethod").Include("Status")
                    where d.Tenant == tenant && d.StatusCode != "DR" && d.StatusCode != "VD" && d.StatusCode != "LL" && d.IsClosed == false
                    select d);
        }

        public IQueryable<ARPayment> GetAccountingLedgerARPayments(int tenant)
        {
            return context.ARPayments.Where(d => d.Tenant == tenant && d.StatusCode != "DR" && d.StatusCode != "VD" && d.StatusCode != "AC");
        }

        public List<ARPayment> GetARPaymentsByBillTo(string billToId, int tenant)
        {
            return (from a in context.ARPayments where a.BillToId == billToId && a.Tenant == tenant select a).ToList();
        }

        public List<ARPayment> GetPaymentsListFromIdList(List<string> ids, int tenant)
        {
            List<ARPayment> list = new List<ARPayment>();

            if (ids.Count > 0)
            {
                list = (from a in context.ARPayments.Include("PaymentCurrency")
                            where a.Tenant == tenant && ids.Contains(a.Id)
                            select a).ToList();
            }

            return list;
        }

        public IQueryable<ARPayment> GetARPayments(int tenant)
        {
            return (from a in context.ARPayments
                    where a.Tenant == tenant
                    select a);
        }
       
        public bool IsARPaymentNumberExists(string arPaymentNo,int tenant)
        {
            return context.ARPayments.Where(d => d.PaymentNo == arPaymentNo && d.Tenant == tenant).Any();
        }

        public double GetARPaymentForCustomer(int tenant, string customerid)
        {
            double? arpayments = (from a in context.ARPayments
                                  where a.BillToId == customerid && a.Tenant == tenant && a.StatusCode != "VD"&&a.StatusCode!="DR"&&a.IsClosed==false&&a.StatusCode!="CL"
                                  select a.OpenAmountInLocalCurrency).Sum();

            return arpayments != null ? arpayments.Value : 0;
        }

        public void Add(ARPayment entity)
        {
            context.ARPayments.Add(entity);
        }

        public void Remove(ARPayment entity)
        {
            context.ARPayments.Attach(entity);
            context.ARPayments.Remove(entity);
        }

        public void Update(ARPayment entity)
        {
            try
            {
                context.ARPayments.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ARPayment> All()
        {
            return context.ARPayments.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ARPayment> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ARPayment GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public string GetARPaymentNumber(string arPaymentId,int tenant)
        {
            string arpaymentno = (from a in context.ARPayments
                                  where a.Tenant == tenant && a.Id == arPaymentId
                                  select a.PaymentNo).FirstOrDefault();
            return arpaymentno;
        }
    }
}
