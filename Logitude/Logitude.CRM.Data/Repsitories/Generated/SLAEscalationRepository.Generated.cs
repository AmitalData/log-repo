 
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
   public partial class SLAEscalationRepository:IRepository<SLAEscalation>
   {
   
        private ICRMContext currentContext;
        public SLAEscalationRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public SLAEscalationRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  SLAEscalation GetSingle(string id, int tenant)
        {
            return (from a in context.SLAEscalations
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SLAEscalation> GetAll(int tenant)
        {
            return from a in context.SLAEscalations  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SLAEscalation GetSingle(EntityKeyFields entityKeys)
        {
            SLAEscalationKeys keys = entityKeys as SLAEscalationKeys;
            return (from a in context.SLAEscalations
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SLAEscalation entity)
        {
            onAdd();
            context.SLAEscalations.Add(entity);
        }

        public void Remove(SLAEscalation entity)
        {
            context.SLAEscalations.Attach(entity);
            context.SLAEscalations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SLAEscalation entity)
        {
            onUpdate();
            context.SLAEscalations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SLAEscalation> All()
        {
            return context.SLAEscalations.ToList();
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
	 