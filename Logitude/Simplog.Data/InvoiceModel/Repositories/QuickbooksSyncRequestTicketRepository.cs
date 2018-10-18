using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;


namespace Simplog.Data.InvoiceModel.Repositories
{
    public class QuickbooksSyncRequestTicketRepository : IRepository<QuickbooksSyncRequestTicket>
    {

          IInvoiceContext invoiceContext;
        public QuickbooksSyncRequestTicketRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public QuickbooksSyncRequestTicketRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public QuickbooksSyncRequestTicketRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<QuickbooksSyncRequestTicket> GetRequestTickets()
        {
            return context.QuickbooksSyncRequestTickets;
        }

        public IQueryable<QuickbooksSyncRequestTicket> GetRequestTickets(int tenant)
        {
            return (from record in context.QuickbooksSyncRequestTickets where record.Tenant == tenant select record);
        }

        public IQueryable<QuickbooksSyncRequestTicket> GetRequestTicketsByTenant(int tenant)
        {
            return (from record in context.QuickbooksSyncRequestTickets where record.Tenant == tenant select record);
        }

     

        public QuickbooksSyncRequestTicket GetSingleRequestTicket(string ticket, int tenant)
        {
            return (from record in context.QuickbooksSyncRequestTickets where record.Ticket == ticket && record.Tenant == tenant select record).FirstOrDefault();          
        }

        public QuickbooksSyncRequestTicket GetSingleRequestTicketByTicket(string ticket)
        {
            return (from record in context.QuickbooksSyncRequestTickets where record.Ticket == ticket  select record).FirstOrDefault();
        }

      
        public void Add(QuickbooksSyncRequestTicket entity)
        {
            context.QuickbooksSyncRequestTickets.Add(entity);
        }

        public void Remove(QuickbooksSyncRequestTicket entity)
        {
            context.QuickbooksSyncRequestTickets.Attach(entity);
            context.QuickbooksSyncRequestTickets.Remove(entity);
        }

        public void Update(QuickbooksSyncRequestTicket entity)
        {
            context.QuickbooksSyncRequestTickets.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuickbooksSyncRequestTicket> All()
        {
            return context.QuickbooksSyncRequestTickets.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<QuickbooksSyncRequestTicket> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QuickbooksSyncRequestTicket GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

    }
}
