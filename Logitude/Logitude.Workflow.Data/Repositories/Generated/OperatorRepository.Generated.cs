 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.Data.Repositories
{
   public partial class OperatorRepository:IRepository<Operator>
   {
   
        private IWorkflowContext currentContext;
        public OperatorRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public OperatorRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  Operator GetSingle(string code)
        {
            return (from a in context.Operators
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<Operator> GetAll()
        {
            return from a in context.Operators  
                   select a;
        }
				 
        public Operator GetSingle(EntityKeyFields entityKeys)
        {
            OperatorKeys keys = entityKeys as OperatorKeys;
            return (from a in context.Operators
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Operator entity)
        {
            onAdd();
            context.Operators.Add(entity);
        }

        public void Remove(Operator entity)
        {
            context.Operators.Attach(entity);
            context.Operators.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Operator entity)
        {
            onUpdate();
            context.Operators.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Operator> All()
        {
            return context.Operators.ToList();
        }

        private IWorkflowContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 