 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.WarehouseLib.Data.Repositories
{
   public partial class WarehouseEntryPackagesReleaseRepository:IRepository<WarehouseEntryPackagesRelease>
   {
   
        private IWarehouseContext currentContext;
        public WarehouseEntryPackagesReleaseRepository(int tenant)
        {
            currentContext = WarehouseContext.GetContext(tenant);
        }

        public WarehouseEntryPackagesReleaseRepository(IWarehouseContext context)
        {
            currentContext = context;
        }

		 
		
		public  WarehouseEntryPackagesRelease GetSingle(string entrypackageid, string releasepackageid, int tenant)
        {
            return (from a in context.WarehouseEntryPackagesReleases
                    where a.EntryPackageId == entrypackageid && a.ReleasePackageId == releasepackageid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<WarehouseEntryPackagesRelease> GetAll(int tenant)
        {
            return from a in context.WarehouseEntryPackagesReleases  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public WarehouseEntryPackagesRelease GetSingle(EntityKeyFields entityKeys)
        {
            WarehouseEntryPackagesReleaseKeys keys = entityKeys as WarehouseEntryPackagesReleaseKeys;
            return (from a in context.WarehouseEntryPackagesReleases
                    where a.EntryPackageId == keys.EntryPackageId && a.ReleasePackageId == keys.ReleasePackageId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WarehouseEntryPackagesRelease entity)
        {
            onAdd();
            context.WarehouseEntryPackagesReleases.Add(entity);
        }

        public void Remove(WarehouseEntryPackagesRelease entity)
        {
            context.WarehouseEntryPackagesReleases.Attach(entity);
            context.WarehouseEntryPackagesReleases.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WarehouseEntryPackagesRelease entity)
        {
            onUpdate();
            context.WarehouseEntryPackagesReleases.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WarehouseEntryPackagesRelease> All()
        {
            return context.WarehouseEntryPackagesReleases.ToList();
        }

        private IWarehouseContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 