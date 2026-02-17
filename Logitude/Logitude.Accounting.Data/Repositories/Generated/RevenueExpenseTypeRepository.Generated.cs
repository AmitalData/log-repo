 
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
   public partial class RevenueExpenseTypeRepository:IRepository<RevenueExpenseType>
   {
   
        private IAccountingContext currentContext;
        public RevenueExpenseTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public RevenueExpenseTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  RevenueExpenseType GetSingle(string code)
        {
            return (from a in context.RevenueExpenseTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RevenueExpenseType> GetAll()
        {
            return from a in context.RevenueExpenseTypes  
                   select a;
        }
				 
        public RevenueExpenseType GetSingle(EntityKeyFields entityKeys)
        {
            RevenueExpenseTypeKeys keys = entityKeys as RevenueExpenseTypeKeys;
            return (from a in context.RevenueExpenseTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RevenueExpenseType entity)
        {
            onAdd();
            context.RevenueExpenseTypes.Add(entity);
        }

        public void Remove(RevenueExpenseType entity)
        {
            context.RevenueExpenseTypes.Attach(entity);
            context.RevenueExpenseTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RevenueExpenseType entity)
        {
            onUpdate();
            context.RevenueExpenseTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RevenueExpenseType> All()
        {
            return context.RevenueExpenseTypes.ToList();
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
	 