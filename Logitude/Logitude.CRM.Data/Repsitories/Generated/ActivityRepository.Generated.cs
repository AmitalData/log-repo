 
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
   public partial class ActivityRepository:IRepository<Activity>
   {
   
        private ICRMContext currentContext;
        public ActivityRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public ActivityRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  Activity GetSingle(string id, int tenant)
        {
            return (from a in context.Activities
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Activity> GetAll(int tenant)
        {
            return from a in context.Activities  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Activity GetSingle(EntityKeyFields entityKeys)
        {
            ActivityKeys keys = entityKeys as ActivityKeys;
            return (from a in context.Activities
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Activity entity)
        {
            onAdd();
            context.Activities.Add(entity);
        }

        public void Remove(Activity entity)
        {
            context.Activities.Attach(entity);
            context.Activities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Activity entity)
        {
            onUpdate();
            context.Activities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Activity> All()
        {
            return context.Activities.ToList();
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
	 