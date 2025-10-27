using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Simplog.Data.InvoiceModel.Repositories
{
   public class ExpenseAllocationFlowRepository : IRepository<ExpenseAllocationFlow>
    {

         IInvoiceContext invoiceContext;
        public ExpenseAllocationFlowRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public ExpenseAllocationFlowRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public ExpenseAllocationFlowRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public ExpenseAllocationFlow GetSingleExpenseAllocationFlow(string id, int tenant)
        {
            return (from a in context.ExpenseAllocationFlows where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public ExpenseAllocationFlow GetSingleById(string id, int tenant)
        {
            return (from a in context.ExpenseAllocationFlows where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }
        

        public IQueryable<ExpenseAllocationFlow> GetExpenseAllocationFlows( int tenant)
        {
            return from a in context.ExpenseAllocationFlows where a.Tenant == tenant select a;
        }
        public IQueryable<ExpenseAllocationFlow> GetListByEntityIAndObjectTable(int tenant,string objectTableId,string entityId)
        {
           return context.ExpenseAllocationFlows
                                      .Join(
                                          context.ExpenseAllocationSettings,
                                          flow => flow.SettingId,        
                                          setting => setting.Id,       
                                          (flow, setting) => new { flow, setting } 
                                      )
                                      .Where(x => x.flow.Tenant == tenant
                                               && x.setting.ObjectTableId == objectTableId
                                               && x.setting.EntityId == entityId 
                                               && x.flow.JournalId != null)
                                      .Select(x => x.flow);
        }
        public void Add(ExpenseAllocationFlow entity)
        {
            context.ExpenseAllocationFlows.Add(entity);
        }
        public void Remove(ExpenseAllocationFlow entity)
        {
            context.ExpenseAllocationFlows.Attach(entity);
            context.ExpenseAllocationFlows.Remove(entity);
        }

        public void Update(ExpenseAllocationFlow entity)
        {
            context.ExpenseAllocationFlows.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExpenseAllocationFlow> All()
        {
            return context.ExpenseAllocationFlows.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<ExpenseAllocationFlow> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ExpenseAllocationFlow GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();

        }



    }
}
