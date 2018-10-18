using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InvoiceModel.Repositories
{
    public class SATInterfaceSettingRepository : IRepository<SATInterfaceSetting>
    {
        IInvoiceContext invoiceContext;
        public SATInterfaceSettingRepository()
        {
            invoiceContext = new InvoiceContext();
        }
        public SATInterfaceSettingRepository(int tenant)
        {
            invoiceContext = InvoiceContext.GetContext(tenant);
        }
        public SATInterfaceSettingRepository(IInvoiceContext context)
        {
            invoiceContext = context;
        }

        public IQueryable<SATInterfaceSetting> GetSATInterfaceSettings(int tenant = 0)
        {
            return context.SATInterfaceSettings;
        }


        public SATInterfaceSetting GetSingleSATInterfaceSetting(int tenantId)
        {
            return (from a in context.SATInterfaceSettings
                    where a.Tenant == tenantId
                    select a).FirstOrDefault();
        }

        public SATInterfaceSetting GetSingleSATInterfaceSetting(int tenantId, int tenant = 0)
        {
            return (from a in context.SATInterfaceSettings
                    where a.Tenant == tenantId
                    select a).FirstOrDefault();
        }


        public void Add(SATInterfaceSetting entity)
        {
            context.SATInterfaceSettings.Add(entity);
        }

        public void Remove(SATInterfaceSetting entity)
        {
            context.SATInterfaceSettings.Attach(entity);
            context.SATInterfaceSettings.Remove(entity);
        }

        public void Update(SATInterfaceSetting entity)
        {
            context.SATInterfaceSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SATInterfaceSetting> All()
        {
            return context.SATInterfaceSettings.ToList();
        }

        public IInvoiceContext context
        {
            get { return invoiceContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<SATInterfaceSetting> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public SATInterfaceSetting GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}