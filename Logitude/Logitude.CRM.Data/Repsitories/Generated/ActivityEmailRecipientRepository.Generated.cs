 
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
   public partial class ActivityEmailRecipientRepository:IRepository<ActivityEmailRecipient>
   {
   
        private ICRMContext currentContext;
        public ActivityEmailRecipientRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public ActivityEmailRecipientRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  ActivityEmailRecipient GetSingle(string id, int tenant)
        {
            return (from a in context.ActivityEmailRecipients
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ActivityEmailRecipient> GetAll(int tenant)
        {
            return from a in context.ActivityEmailRecipients  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ActivityEmailRecipient GetSingle(EntityKeyFields entityKeys)
        {
            ActivityEmailRecipientKeys keys = entityKeys as ActivityEmailRecipientKeys;
            return (from a in context.ActivityEmailRecipients
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ActivityEmailRecipient entity)
        {
            onAdd();
            context.ActivityEmailRecipients.Add(entity);
        }

        public void Remove(ActivityEmailRecipient entity)
        {
            context.ActivityEmailRecipients.Attach(entity);
            context.ActivityEmailRecipients.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ActivityEmailRecipient entity)
        {
            onUpdate();
            context.ActivityEmailRecipients.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ActivityEmailRecipient> All()
        {
            return context.ActivityEmailRecipients.ToList();
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
	 