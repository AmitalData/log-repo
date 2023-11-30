 
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
   public partial class OccasionInviteeRepository:IRepository<OccasionInvitee>
   {
   
        private ICRMContext currentContext;
        public OccasionInviteeRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OccasionInviteeRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  OccasionInvitee GetSingle(string id, int tenant)
        {
            return (from a in context.OccasionInvitees
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<OccasionInvitee> GetAll(int tenant)
        {
            return from a in context.OccasionInvitees  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public OccasionInvitee GetSingle(EntityKeyFields entityKeys)
        {
            OccasionInviteeKeys keys = entityKeys as OccasionInviteeKeys;
            return (from a in context.OccasionInvitees
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OccasionInvitee entity)
        {
            onAdd();
            context.OccasionInvitees.Add(entity);
        }

        public void Remove(OccasionInvitee entity)
        {
            context.OccasionInvitees.Attach(entity);
            context.OccasionInvitees.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OccasionInvitee entity)
        {
            onUpdate();
            context.OccasionInvitees.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OccasionInvitee> All()
        {
            return context.OccasionInvitees.ToList();
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
	 