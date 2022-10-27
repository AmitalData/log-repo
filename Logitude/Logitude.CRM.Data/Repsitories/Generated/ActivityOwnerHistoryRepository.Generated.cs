 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class ActivityOwnerHistoryRepository:IRepository<ActivityOwnerHistory>
   {
   
        private ICRMContext currentContext;
        public ActivityOwnerHistoryRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public ActivityOwnerHistoryRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  ActivityOwnerHistory GetSingle(string id, int tenant)
        {
            return (from a in context.ActivityOwnerHistories
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ActivityOwnerHistory> GetAll(int tenant)
        {
            return from a in context.ActivityOwnerHistories  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ActivityOwnerHistory GetSingle(EntityKeyFields entityKeys)
        {
            ActivityOwnerHistoryKeys keys = entityKeys as ActivityOwnerHistoryKeys;
            return (from a in context.ActivityOwnerHistories
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ActivityOwnerHistory entity)
        {
            onAdd();
            context.ActivityOwnerHistories.Add(entity);
        }

        public void Remove(ActivityOwnerHistory entity)
        {
            context.ActivityOwnerHistories.Attach(entity);
            context.ActivityOwnerHistories.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ActivityOwnerHistory entity)
        {
            onUpdate();
            context.ActivityOwnerHistories.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ActivityOwnerHistory> All()
        {
            return context.ActivityOwnerHistories.ToList();
        }

        private ICRMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 