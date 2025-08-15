using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class PackageConnectedPackageRepository : IRepository<PackageConnectedPackage>
    {
        ICommonDataContext commonDataContext;



        public PackageConnectedPackageRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public PackageConnectedPackageRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<PackageConnectedPackage> GetPackageConnectedPackages()
        {
            return (from d in context.PackageConnectedPackages select d);
        }

        public PackageConnectedPackage GetSinglePackageConnectedPackage(string id)
        {
            return (from d in context.PackageConnectedPackages where d.Id == id select d).FirstOrDefault();

        }

        public void Add(PackageConnectedPackage entity)
        {
            this.context.PackageConnectedPackages.Add(entity);
        }

        public void Remove(PackageConnectedPackage entity)
        {
            this.context.PackageConnectedPackages.Attach(entity);
            this.context.PackageConnectedPackages.Remove(entity);
        }

        public void Update(PackageConnectedPackage entity)
        {
            try
            {
                this.context.PackageConnectedPackages.Attach(entity);
            }

            catch
            {

            }

            this.context.SetAsModified(entity);
        }

        public List<PackageConnectedPackage> All()
        {
            return this.context.PackageConnectedPackages.ToList<PackageConnectedPackage>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<PackageConnectedPackage> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public PackageConnectedPackage GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
