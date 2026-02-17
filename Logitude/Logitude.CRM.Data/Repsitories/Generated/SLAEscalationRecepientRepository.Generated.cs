 
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
   public partial class SLAEscalationRecepientRepository:IRepository<SLAEscalationRecepient>
   {
   
        private ICRMContext currentContext;
        public SLAEscalationRecepientRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public SLAEscalationRecepientRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  SLAEscalationRecepient GetSingle(string id, int tenant)
        {
            return (from a in context.SLAEscalationRecepients
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SLAEscalationRecepient> GetAll(int tenant)
        {
            return from a in context.SLAEscalationRecepients  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SLAEscalationRecepient GetSingle(EntityKeyFields entityKeys)
        {
            SLAEscalationRecepientKeys keys = entityKeys as SLAEscalationRecepientKeys;
            return (from a in context.SLAEscalationRecepients
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SLAEscalationRecepient entity)
        {
            onAdd();
            context.SLAEscalationRecepients.Add(entity);
        }

        public void Remove(SLAEscalationRecepient entity)
        {
            context.SLAEscalationRecepients.Attach(entity);
            context.SLAEscalationRecepients.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SLAEscalationRecepient entity)
        {
            onUpdate();
            context.SLAEscalationRecepients.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SLAEscalationRecepient> All()
        {
            return context.SLAEscalationRecepients.ToList();
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
	 