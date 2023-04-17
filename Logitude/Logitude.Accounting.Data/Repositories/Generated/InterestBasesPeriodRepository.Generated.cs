 
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
   public partial class InterestBasesPeriodRepository:IRepository<InterestBasesPeriod>
   {
   
        private IAccountingContext currentContext;
        public InterestBasesPeriodRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public InterestBasesPeriodRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  InterestBasesPeriod GetSingle(string interestbasetypeid, int linenumber, int tenant)
        {
            return (from a in context.InterestBasesPeriods
                    where a.InterestBaseTypeId == interestbasetypeid && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<InterestBasesPeriod> GetAll(int tenant)
        {
            return from a in context.InterestBasesPeriods  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public InterestBasesPeriod GetSingle(EntityKeyFields entityKeys)
        {
            InterestBasesPeriodKeys keys = entityKeys as InterestBasesPeriodKeys;
            return (from a in context.InterestBasesPeriods
                    where a.InterestBaseTypeId == keys.InterestBaseTypeId && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InterestBasesPeriod entity)
        {
            onAdd();
            context.InterestBasesPeriods.Add(entity);
        }

        public void Remove(InterestBasesPeriod entity)
        {
            context.InterestBasesPeriods.Attach(entity);
            context.InterestBasesPeriods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InterestBasesPeriod entity)
        {
            onUpdate();
            context.InterestBasesPeriods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InterestBasesPeriod> All()
        {
            return context.InterestBasesPeriods.ToList();
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
	 