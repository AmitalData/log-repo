 
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
   public partial class WarehouseEntryRepository:IRepository<WarehouseEntry>
   {
   
        private IWarehouseContext currentContext;
        public WarehouseEntryRepository(int tenant)
        {
            currentContext = WarehouseContext.GetContext(tenant);
        }

        public WarehouseEntryRepository(IWarehouseContext context)
        {
            currentContext = context;
        }

		 
		
		public  WarehouseEntry GetSingle(string id, int tenant)
        {
            return (from a in context.WarehouseEntries
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<WarehouseEntry> GetAll(int tenant)
        {
            return from a in context.WarehouseEntries  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public WarehouseEntry GetSingle(EntityKeyFields entityKeys)
        {
            WarehouseEntryKeys keys = entityKeys as WarehouseEntryKeys;
            return (from a in context.WarehouseEntries
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WarehouseEntry entity)
        {
            onAdd();
            context.WarehouseEntries.Add(entity);
        }

        public void Remove(WarehouseEntry entity)
        {
            context.WarehouseEntries.Attach(entity);
            context.WarehouseEntries.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WarehouseEntry entity)
        {
            onUpdate();
            context.WarehouseEntries.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WarehouseEntry> All()
        {
            return context.WarehouseEntries.ToList();
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
	 