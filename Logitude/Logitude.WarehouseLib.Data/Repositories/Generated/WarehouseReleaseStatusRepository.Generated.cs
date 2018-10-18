 
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
   public partial class WarehouseReleaseStatusRepository:IRepository<WarehouseReleaseStatus>
   {
   
        private IWarehouseContext currentContext;
        public WarehouseReleaseStatusRepository(int tenant)
        {
            currentContext = WarehouseContext.GetContext(tenant);
        }

        public WarehouseReleaseStatusRepository(IWarehouseContext context)
        {
            currentContext = context;
        }

		 
		
		public  WarehouseReleaseStatus GetSingle(string code)
        {
            return (from a in context.WarehouseReleaseStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<WarehouseReleaseStatus> GetAll()
        {
            return from a in context.WarehouseReleaseStatuses  
                   select a;
        }
				 
        public WarehouseReleaseStatus GetSingle(EntityKeyFields entityKeys)
        {
            WarehouseReleaseStatusKeys keys = entityKeys as WarehouseReleaseStatusKeys;
            return (from a in context.WarehouseReleaseStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WarehouseReleaseStatus entity)
        {
            onAdd();
            context.WarehouseReleaseStatuses.Add(entity);
        }

        public void Remove(WarehouseReleaseStatus entity)
        {
            context.WarehouseReleaseStatuses.Attach(entity);
            context.WarehouseReleaseStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WarehouseReleaseStatus entity)
        {
            onUpdate();
            context.WarehouseReleaseStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WarehouseReleaseStatus> All()
        {
            return context.WarehouseReleaseStatuses.ToList();
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
	 