 
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
   public partial class BankPageEntryTypeRepository:IRepository<BankPageEntryType>
   {
   
        private IAccountingContext currentContext;
        public BankPageEntryTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public BankPageEntryTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BankPageEntryType GetSingle(string code)
        {
            return (from a in context.BankPageEntryTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<BankPageEntryType> GetAll()
        {
            return from a in context.BankPageEntryTypes  
                   select a;
        }
				 
        public BankPageEntryType GetSingle(EntityKeyFields entityKeys)
        {
            BankPageEntryTypeKeys keys = entityKeys as BankPageEntryTypeKeys;
            return (from a in context.BankPageEntryTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BankPageEntryType entity)
        {
            onAdd();
            context.BankPageEntryTypes.Add(entity);
        }

        public void Remove(BankPageEntryType entity)
        {
            context.BankPageEntryTypes.Attach(entity);
            context.BankPageEntryTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BankPageEntryType entity)
        {
            onUpdate();
            context.BankPageEntryTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BankPageEntryType> All()
        {
            return context.BankPageEntryTypes.ToList();
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
	 