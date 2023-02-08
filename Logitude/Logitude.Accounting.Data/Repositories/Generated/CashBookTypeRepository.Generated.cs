 
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
   public partial class CashBookTypeRepository:IRepository<CashBookType>
   {
   
        private IAccountingContext currentContext;
        public CashBookTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public CashBookTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CashBookType GetSingle(string code)
        {
            return (from a in context.CashBookTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CashBookType> GetAll()
        {
            return from a in context.CashBookTypes  
                   select a;
        }
				 
        public CashBookType GetSingle(EntityKeyFields entityKeys)
        {
            CashBookTypeKeys keys = entityKeys as CashBookTypeKeys;
            return (from a in context.CashBookTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CashBookType entity)
        {
            onAdd();
            context.CashBookTypes.Add(entity);
        }

        public void Remove(CashBookType entity)
        {
            context.CashBookTypes.Attach(entity);
            context.CashBookTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CashBookType entity)
        {
            onUpdate();
            context.CashBookTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CashBookType> All()
        {
            return context.CashBookTypes.ToList();
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
	 