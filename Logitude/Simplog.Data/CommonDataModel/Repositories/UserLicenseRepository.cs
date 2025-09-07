using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class UserLicenseRepository : IRepository<UserLicense>
    {
        ICommonDataContext commonDataContext;



        public UserLicenseRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public UserLicenseRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<UserLicense> GetUserLicenses(int tenant)
        {
            return (from d in context.UserLicenses where d.Tenant == tenant select d);
        }

        public UserLicense GetSingleUserLicense(string id)
        {
            return (from d in context.UserLicenses where d.Id == id select d).FirstOrDefault();
        }

        public UserLicense GetSingleUserLicenseByUserAndPackage(string userId, string packageCode, int tenant)
        {
            return (from d in context.UserLicenses where d.UserId == userId && d.PackageCode == packageCode && d.Tenant == tenant select d).FirstOrDefault();
        }

        public void Add(UserLicense entity)
        {
            this.context.UserLicenses.Add(entity);
        }

        public void Remove(UserLicense entity)
        {
            this.context.UserLicenses.Attach(entity);
            this.context.UserLicenses.Remove(entity);
        }

        public void Update(UserLicense entity)
        {
            try
            {
                this.context.UserLicenses.Attach(entity);
            }

            catch
            {

            }

            this.context.SetAsModified(entity);
        }

        public List<UserLicense> All()
        {
            return this.context.UserLicenses.ToList<UserLicense>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<UserLicense> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public UserLicense GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public List<UserLicense> GetUserLicensesByUserId(string userId, int tenant)
        {
            return (from d in context.UserLicenses where d.Tenant == tenant && d.UserId == userId select d).ToList();
        }

        public int? GetUserLicensesCountByPackageCode(string packageCode,int tenant)
        {
            return (from d in context.UserLicenses where d.PackageCode == packageCode && d.Tenant == tenant select d).Count();
        }

        public List<UserLicense> GetUserLicensesByPackageCode(string packageCode, int tenant)
        {
            return (from d in context.UserLicenses where d.Tenant == tenant && d.PackageCode == packageCode select d).ToList();
        }
    }
}
