using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Repositories
{
   public class ARPaymentChequeStatusReplicaRepository : IRepository<ARPaymentChequeStatusReplica>
    {

        IInvoiceContext invoiceContext;
        public ARPaymentChequeStatusReplicaRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public ARPaymentChequeStatusReplicaRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public ARPaymentChequeStatusReplicaRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<ARPaymentChequeStatusReplica> GetARPaymentChequeStatusReplicas()
        {
            return context.ARPaymentChequeStatusReplicas;
        }

        public IQueryable<ARPaymentChequeStatusReplica> GetAll()
        {
            return context.ARPaymentChequeStatusReplicas;
        }

        public ARPaymentChequeStatusReplica GetSingleARPaymentChequeStatusReplica(string code)
        {
            return (from a in context.ARPaymentChequeStatusReplicas
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(ARPaymentChequeStatusReplica entity)
        {
            context.ARPaymentChequeStatusReplicas.Add(entity);
        }

        public void Remove(ARPaymentChequeStatusReplica entity)
        {
            context.ARPaymentChequeStatusReplicas.Attach(entity);
            context.ARPaymentChequeStatusReplicas.Remove(entity);
        }

        public void Update(ARPaymentChequeStatusReplica entity)
        {
            context.ARPaymentChequeStatusReplicas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ARPaymentChequeStatusReplica> All()
        {
            return context.ARPaymentChequeStatusReplicas.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ARPaymentChequeStatusReplica> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ARPaymentChequeStatusReplica GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}
