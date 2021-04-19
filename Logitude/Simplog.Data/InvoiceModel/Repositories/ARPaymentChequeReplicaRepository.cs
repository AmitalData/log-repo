using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.InvoiceModel.Repositories
{
   public class ARPaymentChequeReplicaRepository : IRepository<ARPaymentChequeReplica>
    {

        IInvoiceContext invoiceContext;

        public ARPaymentChequeReplicaRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public ARPaymentChequeReplicaRepository()
        {
            invoiceContext = new InvoiceContext();
        }

        public ARPaymentChequeReplicaRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }

        public ARPaymentChequeReplica GetSingleARPaymentChequeReplica(string id, int tenant)
        {
            return (from a in context.ARPaymentChequeReplicas where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public bool ChequeIfPaymentChequeReplicaExist(string paymentId, string chequeNumber,int lineNo, int tenant)
        {
            return (from a in context.ARPaymentChequeReplicas where a.PaymentId == paymentId && a.Tenant == tenant && a.ChequeNumber == chequeNumber && a.LineNumber == lineNo select a).Any();
        }
       
        public IQueryable<ARPaymentChequeReplica> GetARPaymentChequeReplicas(string paymentId, int tenant)
        {
            return (from a in context.ARPaymentChequeReplicas where a.PaymentId == paymentId && a.Tenant == tenant select a).Include("ARPaymentChequeStatusReplica");
        }

        public void Add(ARPaymentChequeReplica entity)
        {
            context.ARPaymentChequeReplicas.Add(entity);
        }

        public void Remove(ARPaymentChequeReplica entity)
        {
            context.ARPaymentChequeReplicas.Attach(entity);
            context.ARPaymentChequeReplicas.Remove(entity);
        }

        public void Update(ARPaymentChequeReplica entity)
        {
            try
            {
                context.ARPaymentChequeReplicas.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ARPaymentChequeReplica> All()
        {
            return context.ARPaymentChequeReplicas.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ARPaymentChequeReplica> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ARPaymentChequeReplica GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }




    }
}
