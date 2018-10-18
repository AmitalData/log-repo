 
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
   public partial class ActivityPriorityRepository:IRepository<ActivityPriority>
   {
   
        private ICRMContext currentContext;
        public ActivityPriorityRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public ActivityPriorityRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  ActivityPriority GetSingle(string code)
        {
            return (from a in context.ActivityPriorities
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ActivityPriority> GetAll()
        {
            return from a in context.ActivityPriorities  
                   select a;
        }
				 
        public ActivityPriority GetSingle(EntityKeyFields entityKeys)
        {
            ActivityPriorityKeys keys = entityKeys as ActivityPriorityKeys;
            return (from a in context.ActivityPriorities
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ActivityPriority entity)
        {
            onAdd();
            context.ActivityPriorities.Add(entity);
        }

        public void Remove(ActivityPriority entity)
        {
            context.ActivityPriorities.Attach(entity);
            context.ActivityPriorities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ActivityPriority entity)
        {
            onUpdate();
            context.ActivityPriorities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ActivityPriority> All()
        {
            return context.ActivityPriorities.ToList();
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
	 