 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class ConsignmentInternalTransitionRepository:IRepository<ConsignmentInternalTransition>
   {
   
        private ICustomContext currentContext;
        public ConsignmentInternalTransitionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ConsignmentInternalTransitionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConsignmentInternalTransition GetSingle(string declarationid, int? consignmentnumber, int linenumber, int tenant)
        {
            return (from a in context.ConsignmentInternalTransitions
                    where a.DeclarationId == declarationid && a.ConsignmentNumber == consignmentnumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ConsignmentInternalTransition> GetAll(int tenant)
        {
            return from a in context.ConsignmentInternalTransitions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ConsignmentInternalTransition GetSingle(EntityKeyFields entityKeys)
        {
            ConsignmentInternalTransitionKeys keys = entityKeys as ConsignmentInternalTransitionKeys;
            return (from a in context.ConsignmentInternalTransitions
                    where a.DeclarationId == keys.DeclarationId && a.ConsignmentNumber == keys.ConsignmentNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConsignmentInternalTransition entity)
        {
            onAdd();
            context.ConsignmentInternalTransitions.Add(entity);
        }

        public void Remove(ConsignmentInternalTransition entity)
        {
            context.ConsignmentInternalTransitions.Attach(entity);
            context.ConsignmentInternalTransitions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConsignmentInternalTransition entity)
        {
            onUpdate();
            context.ConsignmentInternalTransitions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConsignmentInternalTransition> All()
        {
            return context.ConsignmentInternalTransitions.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 