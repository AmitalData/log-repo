 
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
   public partial class ConstraintApprovalDecisionRepository:IRepository<ConstraintApprovalDecision>
   {
   
        private ICustomContext currentContext;
        public ConstraintApprovalDecisionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ConstraintApprovalDecisionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConstraintApprovalDecision GetSingle(string code)
        {
            return (from a in context.ConstraintApprovalDecisions
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ConstraintApprovalDecision> GetAll()
        {
            return from a in context.ConstraintApprovalDecisions  
                   select a;
        }
				 
        public ConstraintApprovalDecision GetSingle(EntityKeyFields entityKeys)
        {
            ConstraintApprovalDecisionKeys keys = entityKeys as ConstraintApprovalDecisionKeys;
            return (from a in context.ConstraintApprovalDecisions
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConstraintApprovalDecision entity)
        {
            onAdd();
            context.ConstraintApprovalDecisions.Add(entity);
        }

        public void Remove(ConstraintApprovalDecision entity)
        {
            context.ConstraintApprovalDecisions.Attach(entity);
            context.ConstraintApprovalDecisions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConstraintApprovalDecision entity)
        {
            onUpdate();
            context.ConstraintApprovalDecisions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConstraintApprovalDecision> All()
        {
            return context.ConstraintApprovalDecisions.ToList();
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
	 