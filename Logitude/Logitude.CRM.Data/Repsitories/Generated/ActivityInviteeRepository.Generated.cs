 
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
   public partial class ActivityInviteeRepository:IRepository<ActivityInvitee>
   {
   
        private ICRMContext currentContext;
        public ActivityInviteeRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public ActivityInviteeRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  ActivityInvitee GetSingle(string id, int tenant)
        {
            return (from a in context.ActivityInvitees
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ActivityInvitee> GetAll(int tenant)
        {
            return from a in context.ActivityInvitees  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ActivityInvitee GetSingle(EntityKeyFields entityKeys)
        {
            ActivityInviteeKeys keys = entityKeys as ActivityInviteeKeys;
            return (from a in context.ActivityInvitees
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ActivityInvitee entity)
        {
            onAdd();
            context.ActivityInvitees.Add(entity);
        }

        public void Remove(ActivityInvitee entity)
        {
            context.ActivityInvitees.Attach(entity);
            context.ActivityInvitees.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ActivityInvitee entity)
        {
            onUpdate();
            context.ActivityInvitees.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ActivityInvitee> All()
        {
            return context.ActivityInvitees.ToList();
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
	 