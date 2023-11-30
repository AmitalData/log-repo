 
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
   public partial class OperatorCategoryRepository:IRepository<OperatorCategory>
   {
   
        private IWorkflowContext currentContext;
        public OperatorCategoryRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public OperatorCategoryRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  OperatorCategory GetSingle(string code)
        {
            return (from a in context.OperatorCategories
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<OperatorCategory> GetAll()
        {
            return from a in context.OperatorCategories  
                   select a;
        }
				 
        public OperatorCategory GetSingle(EntityKeyFields entityKeys)
        {
            OperatorCategoryKeys keys = entityKeys as OperatorCategoryKeys;
            return (from a in context.OperatorCategories
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OperatorCategory entity)
        {
            onAdd();
            context.OperatorCategories.Add(entity);
        }

        public void Remove(OperatorCategory entity)
        {
            context.OperatorCategories.Attach(entity);
            context.OperatorCategories.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OperatorCategory entity)
        {
            onUpdate();
            context.OperatorCategories.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OperatorCategory> All()
        {
            return context.OperatorCategories.ToList();
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
	 