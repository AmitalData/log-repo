using System.Collections.Generic;
using System.Linq;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
   public class ExpenseAllocationSettingRepository : IRepository<ExpenseAllocationSetting>
    {

         IInvoiceContext invoiceContext;
        public ExpenseAllocationSettingRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public ExpenseAllocationSettingRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public ExpenseAllocationSettingRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public ExpenseAllocationSetting GetSingleByEntityIdAndObjectTable(string entityId, string objectTableId,int tenant)
        {
            return (from record in context.ExpenseAllocationSettings where record.EntityId == entityId && record.Tenant == tenant && record.ObjectTableId == objectTableId select record).FirstOrDefault();          
        }
         public void Add(ExpenseAllocationSetting entity)
        {
            context.ExpenseAllocationSettings.Add(entity);
        }
        public void Remove(ExpenseAllocationSetting entity)
        {
            context.ExpenseAllocationSettings.Attach(entity);
            context.ExpenseAllocationSettings.Remove(entity);
        }

        public void Update(ExpenseAllocationSetting entity)
        {
            context.ExpenseAllocationSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExpenseAllocationSetting> All()
        {
            return context.ExpenseAllocationSettings.ToList();
        }

        public IInvoiceContext context
        {
            get {return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<ExpenseAllocationSetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ExpenseAllocationSetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }


     
    }
}
