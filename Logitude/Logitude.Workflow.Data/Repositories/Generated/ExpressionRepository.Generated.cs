 
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
   public partial class ExpressionRepository:IRepository<Expression>
   {
   
        private IWorkflowContext currentContext;
        public ExpressionRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public ExpressionRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  Expression GetSingle(string code)
        {
            return (from a in context.Expressions
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<Expression> GetAll()
        {
            return from a in context.Expressions  
                   select a;
        }
				 
        public Expression GetSingle(EntityKeyFields entityKeys)
        {
            ExpressionKeys keys = entityKeys as ExpressionKeys;
            return (from a in context.Expressions
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Expression entity)
        {
            onAdd();
            context.Expressions.Add(entity);
        }

        public void Remove(Expression entity)
        {
            context.Expressions.Attach(entity);
            context.Expressions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Expression entity)
        {
            onUpdate();
            context.Expressions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Expression> All()
        {
            return context.Expressions.ToList();
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
	 