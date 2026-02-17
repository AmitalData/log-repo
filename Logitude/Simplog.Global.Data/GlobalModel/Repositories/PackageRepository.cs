using System.Collections.Generic;
using System.Linq;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class PackageRepository:IRepository<Package>
	{
		IGlobalContext globalContext;

		public IGlobalContext context
		{
			get { return globalContext; }
		}
		public PackageRepository(IGlobalContext context)
		{
			globalContext = context;
		}

		public PackageRepository()
		{
			globalContext = GlobalContext.GetContext();
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
