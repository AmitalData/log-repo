 
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
   public partial class ActivityTypeRepository:IRepository<ActivityType>
   {
   
        private ICRMContext currentContext;
        public ActivityTypeRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public ActivityTypeRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  ActivityType GetSingle(string code)
        {
            return (from a in context.ActivityTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ActivityType> GetAll()
        {
            return from a in context.ActivityTypes  
                   select a;
        }
				 
        public ActivityType GetSingle(EntityKeyFields entityKeys)
        {
            ActivityTypeKeys keys = entityKeys as ActivityTypeKeys;
            return (from a in context.ActivityTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ActivityType entity)
        {
            onAdd();
            context.ActivityTypes.Add(entity);
        }

        public void Remove(ActivityType entity)
        {
            context.ActivityTypes.Attach(entity);
            context.ActivityTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ActivityType entity)
        {
            onUpdate();
            context.ActivityTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ActivityType> All()
        {
            return context.ActivityTypes.ToList();
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
	 