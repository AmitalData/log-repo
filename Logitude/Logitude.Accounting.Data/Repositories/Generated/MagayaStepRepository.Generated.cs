 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class MagayaStepRepository:IRepository<MagayaStep>
   {
   
        private IAccountingContext currentContext;
        public MagayaStepRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public MagayaStepRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  MagayaStep GetSingle(string code)
        {
            return (from a in context.MagayaSteps
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<MagayaStep> GetAll()
        {
            return from a in context.MagayaSteps  
                   select a;
        }
				 
        public MagayaStep GetSingle(EntityKeyFields entityKeys)
        {
            MagayaStepKeys keys = entityKeys as MagayaStepKeys;
            return (from a in context.MagayaSteps
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(MagayaStep entity)
        {
            onAdd();
            context.MagayaSteps.Add(entity);
        }

        public void Remove(MagayaStep entity)
        {
            context.MagayaSteps.Attach(entity);
            context.MagayaSteps.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(MagayaStep entity)
        {
            onUpdate();
            context.MagayaSteps.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MagayaStep> All()
        {
            return context.MagayaSteps.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 