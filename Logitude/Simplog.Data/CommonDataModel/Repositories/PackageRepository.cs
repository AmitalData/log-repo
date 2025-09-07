using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.CommonDataModel.Repositories
{
    public class PackageRepository:IRepository<Package>
    {
        ICommonDataContext commonDataContext;



        public PackageRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public PackageRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Package> GetPackages()
        {
            return (from record in context.Packages select record);
        }

        public Package GetSinglePackage(string code)
        {
            return (from record in context.Packages where record.Code == code  select record).FirstOrDefault();

        }

        public void Add(Package entity)
        {
            this.context.Packages.Add(entity);
        }

        public void Remove(Package entity)
        {
            this.context.Packages.Attach(entity);
            this.context.Packages.Remove(entity);
        }

        public void Update(Package entity)
        {
            try
            {
                this.context.Packages.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<Package> All()
        {
            return this.context.Packages.ToList<Package>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<Package> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Package GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
