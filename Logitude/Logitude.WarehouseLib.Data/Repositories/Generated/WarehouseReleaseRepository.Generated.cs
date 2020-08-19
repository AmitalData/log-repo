 
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
   public partial class WarehouseReleaseRepository:IRepository<WarehouseRelease>
   {
   
        private IWarehouseContext currentContext;
        public WarehouseReleaseRepository(int tenant)
        {
            currentContext = WarehouseContext.GetContext(tenant);
        }

        public WarehouseReleaseRepository(IWarehouseContext context)
        {
            currentContext = context;
        }

		 
		
		public  WarehouseRelease GetSingle(string id, int tenant)
        {
            return (from a in context.WarehouseReleases
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<WarehouseRelease> GetAll(int tenant)
        {
            return from a in context.WarehouseReleases  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public WarehouseRelease GetSingle(EntityKeyFields entityKeys)
        {
            WarehouseReleaseKeys keys = entityKeys as WarehouseReleaseKeys;
            return (from a in context.WarehouseReleases
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WarehouseRelease entity)
        {
            onAdd();
            context.WarehouseReleases.Add(entity);
        }

        public void Remove(WarehouseRelease entity)
        {
            context.WarehouseReleases.Attach(entity);
            context.WarehouseReleases.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WarehouseRelease entity)
        {
            onUpdate();
            context.WarehouseReleases.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WarehouseRelease> All()
        {
            return context.WarehouseReleases.ToList();
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
	 