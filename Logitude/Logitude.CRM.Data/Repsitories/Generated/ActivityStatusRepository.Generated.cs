 
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
   public partial class ActivityStatusRepository:IRepository<ActivityStatus>
   {
   
        private ICRMContext currentContext;
        public ActivityStatusRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public ActivityStatusRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  ActivityStatus GetSingle(string code)
        {
            return (from a in context.ActivityStatus
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ActivityStatus> GetAll()
        {
            return from a in context.ActivityStatus  
                   select a;
        }
				 
        public ActivityStatus GetSingle(EntityKeyFields entityKeys)
        {
            ActivityStatusKeys keys = entityKeys as ActivityStatusKeys;
            return (from a in context.ActivityStatus
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ActivityStatus entity)
        {
            onAdd();
            context.ActivityStatus.Add(entity);
        }

        public void Remove(ActivityStatus entity)
        {
            context.ActivityStatus.Attach(entity);
            context.ActivityStatus.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ActivityStatus entity)
        {
            onUpdate();
            context.ActivityStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ActivityStatus> All()
        {
            return context.ActivityStatus.ToList();
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
	 