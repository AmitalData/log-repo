using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class AccountingSystemsSettingRepository : IRepository<AccountingSystemsSetting>
    {
           IInvoiceContext invoiceContext;
        public AccountingSystemsSettingRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public AccountingSystemsSettingRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public AccountingSystemsSettingRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<AccountingSystemsSetting> GetAccountingSystemsSettings()
        {
            return context.AccountingSystemsSettings;
        }

        public IQueryable<AccountingSystemsSetting> GetAccountingSystemsSettings(int tenant)
        {
            return (from record in context.AccountingSystemsSettings where record.Tenant == tenant select record);
        }

        public IQueryable<AccountingSystemsSetting> GetAccountingSystemsSettingsByTenant(int tenant)
        {
            return (from record in context.AccountingSystemsSettings where record.Tenant == tenant select record);
        }



        public AccountingSystemsSetting GetSingleAccountingSystemsSetting(string id, int tenant)
        {
            return (from record in context.AccountingSystemsSettings where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public AccountingSystemsSetting GetSingleAccountingSystemsSetting( int tenant)
        {
            return (from record in context.AccountingSystemsSettings where  record.Tenant == tenant select record).FirstOrDefault();
        }

      

        public AccountingSystemsSetting GetSingleAccountingSystemsSettingByCode(string id, int tenant)
        {
            return (from record in context.AccountingSystemsSettings where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }


        public void Add(AccountingSystemsSetting entity)
        {
            context.AccountingSystemsSettings.Add(entity);
        }

        public void Remove(AccountingSystemsSetting entity)
        {
            context.AccountingSystemsSettings.Attach(entity);
            context.AccountingSystemsSettings.Remove(entity);
        }

        public void Update(AccountingSystemsSetting entity)
        {
            context.AccountingSystemsSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountingSystemsSetting> All()
        {
            return context.AccountingSystemsSettings.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }






        public List<AccountingSystemsSetting> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public AccountingSystemsSetting GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
