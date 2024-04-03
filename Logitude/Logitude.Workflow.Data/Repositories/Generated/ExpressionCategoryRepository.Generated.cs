 
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
   public partial class ExpressionCategoryRepository:IRepository<ExpressionCategory>
   {
   
        private IWorkflowContext currentContext;
        public ExpressionCategoryRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public ExpressionCategoryRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExpressionCategory GetSingle(string code)
        {
            return (from a in context.ExpressionCategories
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ExpressionCategory> GetAll()
        {
            return from a in context.ExpressionCategories  
                   select a;
        }
				 
        public ExpressionCategory GetSingle(EntityKeyFields entityKeys)
        {
            ExpressionCategoryKeys keys = entityKeys as ExpressionCategoryKeys;
            return (from a in context.ExpressionCategories
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExpressionCategory entity)
        {
            onAdd();
            context.ExpressionCategories.Add(entity);
        }

        public void Remove(ExpressionCategory entity)
        {
            context.ExpressionCategories.Attach(entity);
            context.ExpressionCategories.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExpressionCategory entity)
        {
            onUpdate();
            context.ExpressionCategories.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExpressionCategory> All()
        {
            return context.ExpressionCategories.ToList();
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
	 