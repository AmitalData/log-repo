 
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
   public partial class WarehouseEntryPackageRepository:IRepository<WarehouseEntryPackage>
   {
   
        private IWarehouseContext currentContext;
        public WarehouseEntryPackageRepository(int tenant)
        {
            currentContext = WarehouseContext.GetContext(tenant);
        }

        public WarehouseEntryPackageRepository(IWarehouseContext context)
        {
            currentContext = context;
        }

		 
		
		public  WarehouseEntryPackage GetSingle(string id, int tenant)
        {
            return (from a in context.WarehouseEntryPackages
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<WarehouseEntryPackage> GetAll(int tenant)
        {
            return from a in context.WarehouseEntryPackages  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public WarehouseEntryPackage GetSingle(EntityKeyFields entityKeys)
        {
            WarehouseEntryPackageKeys keys = entityKeys as WarehouseEntryPackageKeys;
            return (from a in context.WarehouseEntryPackages
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WarehouseEntryPackage entity)
        {
            onAdd();
            context.WarehouseEntryPackages.Add(entity);
        }

        public void Remove(WarehouseEntryPackage entity)
        {
            context.WarehouseEntryPackages.Attach(entity);
            context.WarehouseEntryPackages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WarehouseEntryPackage entity)
        {
            onUpdate();
            context.WarehouseEntryPackages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WarehouseEntryPackage> All()
        {
            return context.WarehouseEntryPackages.ToList();
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
	 