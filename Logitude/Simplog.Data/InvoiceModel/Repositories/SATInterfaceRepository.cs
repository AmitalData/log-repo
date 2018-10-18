using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class SATInterfaceRepository : IRepository<SATInterface>
    {
        IInvoiceContext invoiceContext;
        public SATInterfaceRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public SATInterfaceRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public SATInterfaceRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<SATInterface> GetSATInterfaces()
        {
            return context.SATInterfaces;
        }

        public IQueryable<SATInterface> GetAll()
        {
            return context.SATInterfaces;
        }

        public SATInterface GetSingleSATInterface(string code)
        {
            return (from a in context.SATInterfaces
                    where a.Code == code
                    select a).FirstOrDefault();
        }


        public void Add(SATInterface entity)
        {
            context.SATInterfaces.Add(entity);
        }

        public void Remove(SATInterface entity)
        {
            context.SATInterfaces.Attach(entity);
            context.SATInterfaces.Remove(entity);
        }

        public void Update(SATInterface entity)
        {
            context.SATInterfaces.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SATInterface> All()
        {
            return context.SATInterfaces.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<SATInterface> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public SATInterface GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}