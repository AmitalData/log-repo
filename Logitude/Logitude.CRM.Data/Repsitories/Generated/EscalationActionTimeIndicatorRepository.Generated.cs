 
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
   public partial class EscalationActionTimeIndicatorRepository:IRepository<EscalationActionTimeIndicator>
   {
   
        private ICRMContext currentContext;
        public EscalationActionTimeIndicatorRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public EscalationActionTimeIndicatorRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  EscalationActionTimeIndicator GetSingle(string code)
        {
            return (from a in context.EscalationActionTimeIndicators
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<EscalationActionTimeIndicator> GetAll()
        {
            return from a in context.EscalationActionTimeIndicators  
                   select a;
        }
				 
        public EscalationActionTimeIndicator GetSingle(EntityKeyFields entityKeys)
        {
            EscalationActionTimeIndicatorKeys keys = entityKeys as EscalationActionTimeIndicatorKeys;
            return (from a in context.EscalationActionTimeIndicators
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(EscalationActionTimeIndicator entity)
        {
            onAdd();
            context.EscalationActionTimeIndicators.Add(entity);
        }

        public void Remove(EscalationActionTimeIndicator entity)
        {
            context.EscalationActionTimeIndicators.Attach(entity);
            context.EscalationActionTimeIndicators.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(EscalationActionTimeIndicator entity)
        {
            onUpdate();
            context.EscalationActionTimeIndicators.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<EscalationActionTimeIndicator> All()
        {
            return context.EscalationActionTimeIndicators.ToList();
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
	 