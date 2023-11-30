 
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
   public partial class WarehouseEntryStatusRepository:IRepository<WarehouseEntryStatus>
   {
   
        private IWarehouseContext currentContext;
        public WarehouseEntryStatusRepository(int tenant)
        {
            currentContext = WarehouseContext.GetContext(tenant);
        }

        public WarehouseEntryStatusRepository(IWarehouseContext context)
        {
            currentContext = context;
        }

		 
		
		public  WarehouseEntryStatus GetSingle(string code)
        {
            return (from a in context.WarehouseEntryStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<WarehouseEntryStatus> GetAll()
        {
            return from a in context.WarehouseEntryStatuses  
                   select a;
        }
				 
        public WarehouseEntryStatus GetSingle(EntityKeyFields entityKeys)
        {
            WarehouseEntryStatusKeys keys = entityKeys as WarehouseEntryStatusKeys;
            return (from a in context.WarehouseEntryStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WarehouseEntryStatus entity)
        {
            onAdd();
            context.WarehouseEntryStatuses.Add(entity);
        }

        public void Remove(WarehouseEntryStatus entity)
        {
            context.WarehouseEntryStatuses.Attach(entity);
            context.WarehouseEntryStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WarehouseEntryStatus entity)
        {
            onUpdate();
            context.WarehouseEntryStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WarehouseEntryStatus> All()
        {
            return context.WarehouseEntryStatuses.ToList();
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
	 