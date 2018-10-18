using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class TenantManagementLicenseRepository : IRepository<TenantManagementLicense>
    {
        IGlobalContext globalContext;

        public TenantManagementLicenseRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public TenantManagementLicenseRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public TenantManagementLicenseRepository(int tenant)
        {
            globalContext = GlobalContext.GetContext();
        }

        public IQueryable<TenantManagementLicense> GetTenantManagementLicenses(int tenant)
        {
            return (from d in context.TenantManagementLicenses where d.Tenant == tenant select d);
        }

        public TenantManagementLicense GetSingleTenantManagementLicense(string id)
        {
            return (from d in context.TenantManagementLicenses where d.Id == id select d).FirstOrDefault();

        }

        public void Add(TenantManagementLicense entity)
        {
            this.context.TenantManagementLicenses.Add(entity);
        }

        public void Remove(TenantManagementLicense entity)
        {
            this.context.TenantManagementLicenses.Attach(entity);
            this.context.TenantManagementLicenses.Remove(entity);
        }

        public void Update(TenantManagementLicense entity)
        {
            try
            {
                this.context.TenantManagementLicenses.Attach(entity);
            }

            catch
            {

            }

            this.context.SetAsModified(entity);
        }

        public List<TenantManagementLicense> All()
        {
            return this.context.TenantManagementLicenses.ToList<TenantManagementLicense>();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<TenantManagementLicense> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TenantManagementLicense GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
