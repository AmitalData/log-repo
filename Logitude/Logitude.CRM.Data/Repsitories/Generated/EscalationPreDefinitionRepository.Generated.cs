 
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
   public partial class EscalationPreDefinitionRepository:IRepository<EscalationPreDefinition>
   {
   
        private ICRMContext currentContext;
        public EscalationPreDefinitionRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public EscalationPreDefinitionRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  EscalationPreDefinition GetSingle(string code)
        {
            return (from a in context.EscalationPreDefinitions
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<EscalationPreDefinition> GetAll()
        {
            return from a in context.EscalationPreDefinitions  
                   select a;
        }
				 
        public EscalationPreDefinition GetSingle(EntityKeyFields entityKeys)
        {
            EscalationPreDefinitionKeys keys = entityKeys as EscalationPreDefinitionKeys;
            return (from a in context.EscalationPreDefinitions
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(EscalationPreDefinition entity)
        {
            onAdd();
            context.EscalationPreDefinitions.Add(entity);
        }

        public void Remove(EscalationPreDefinition entity)
        {
            context.EscalationPreDefinitions.Attach(entity);
            context.EscalationPreDefinitions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(EscalationPreDefinition entity)
        {
            onUpdate();
            context.EscalationPreDefinitions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<EscalationPreDefinition> All()
        {
            return context.EscalationPreDefinitions.ToList();
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
	 