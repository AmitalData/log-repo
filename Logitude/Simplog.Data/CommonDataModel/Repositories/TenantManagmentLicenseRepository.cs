using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TenantManagmentLicenseRepository : IRepository<TenantManagmentLicense>
    {
        ICommonDataContext commonDataContext;

        public TenantManagmentLicenseRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public TenantManagmentLicenseRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TenantManagmentLicenseRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<TenantManagmentLicense> GetTenantManagmentLicenses(int tenant)
        {
            return (from d in context.TenantManagmentLicenses where d.Tenant == tenant select d);
        }

        public TenantManagmentLicense GetSingleTenantManagmentLicense(string id)
        {
            return (from d in context.TenantManagmentLicenses where d.Id == id select d).FirstOrDefault();

        }

        public void Add(TenantManagmentLicense entity)
        {
            this.context.TenantManagmentLicenses.Add(entity);
        }

        public void Remove(TenantManagmentLicense entity)
        {
            this.context.TenantManagmentLicenses.Attach(entity);
            this.context.TenantManagmentLicenses.Remove(entity);
        }

        public void Update(TenantManagmentLicense entity)
        {
            try
            {
                this.context.TenantManagmentLicenses.Attach(entity);
            }

            catch
            {

            }

            this.context.SetAsModified(entity);
        }

        public List<TenantManagmentLicense> All()
        {
            return this.context.TenantManagmentLicenses.ToList<TenantManagmentLicense>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<TenantManagmentLicense> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TenantManagmentLicense GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
