using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class PackageConnectedPackageRepository : IRepository<PackageConnectedPackage>
    {

		IGlobalContext globalContext;

		public IGlobalContext context
		{
			get { return globalContext; }
		}
		public PackageConnectedPackageRepository(IGlobalContext context)
		{
			globalContext = context;
		}

		public PackageConnectedPackageRepository()
		{
			globalContext = GlobalContext.GetContext();
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
