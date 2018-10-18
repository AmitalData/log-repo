 
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
   public partial class WarehouseReleasePackageRepository:IRepository<WarehouseReleasePackage>
   {
   
        private IWarehouseContext currentContext;
        public WarehouseReleasePackageRepository(int tenant)
        {
            currentContext = WarehouseContext.GetContext(tenant);
        }

        public WarehouseReleasePackageRepository(IWarehouseContext context)
        {
            currentContext = context;
        }

		 
		
		public  WarehouseReleasePackage GetSingle(string id, int tenant)
        {
            return (from a in context.WarehouseReleasePackages
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<WarehouseReleasePackage> GetAll(int tenant)
        {
            return from a in context.WarehouseReleasePackages  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public WarehouseReleasePackage GetSingle(EntityKeyFields entityKeys)
        {
            WarehouseReleasePackageKeys keys = entityKeys as WarehouseReleasePackageKeys;
            return (from a in context.WarehouseReleasePackages
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WarehouseReleasePackage entity)
        {
            onAdd();
            context.WarehouseReleasePackages.Add(entity);
        }

        public void Remove(WarehouseReleasePackage entity)
        {
            context.WarehouseReleasePackages.Attach(entity);
            context.WarehouseReleasePackages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WarehouseReleasePackage entity)
        {
            onUpdate();
            context.WarehouseReleasePackages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WarehouseReleasePackage> All()
        {
            return context.WarehouseReleasePackages.ToList();
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
	 