 
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
   public partial class PeriodTypeRepository:IRepository<PeriodType>
   {
   
        private IAccountingContext currentContext;
        public PeriodTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public PeriodTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  PeriodType GetSingle(string code)
        {
            return (from a in context.PeriodTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PeriodType> GetAll()
        {
            return from a in context.PeriodTypes  
                   select a;
        }
				 
        public PeriodType GetSingle(EntityKeyFields entityKeys)
        {
            PeriodTypeKeys keys = entityKeys as PeriodTypeKeys;
            return (from a in context.PeriodTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PeriodType entity)
        {
            onAdd();
            context.PeriodTypes.Add(entity);
        }

        public void Remove(PeriodType entity)
        {
            context.PeriodTypes.Attach(entity);
            context.PeriodTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PeriodType entity)
        {
            onUpdate();
            context.PeriodTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PeriodType> All()
        {
            return context.PeriodTypes.ToList();
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
	 