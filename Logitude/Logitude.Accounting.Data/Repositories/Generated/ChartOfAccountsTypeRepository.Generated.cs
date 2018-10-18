 
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
   public partial class ChartOfAccountsTypeRepository:IRepository<ChartOfAccountsType>
   {
   
        private IAccountingContext currentContext;
        public ChartOfAccountsTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ChartOfAccountsTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ChartOfAccountsType GetSingle(string code)
        {
            return (from a in context.ChartOfAccountsTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ChartOfAccountsType> GetAll()
        {
            return from a in context.ChartOfAccountsTypes  
                   select a;
        }
				 
        public ChartOfAccountsType GetSingle(EntityKeyFields entityKeys)
        {
            ChartOfAccountsTypeKeys keys = entityKeys as ChartOfAccountsTypeKeys;
            return (from a in context.ChartOfAccountsTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ChartOfAccountsType entity)
        {
            onAdd();
            context.ChartOfAccountsTypes.Add(entity);
        }

        public void Remove(ChartOfAccountsType entity)
        {
            context.ChartOfAccountsTypes.Attach(entity);
            context.ChartOfAccountsTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ChartOfAccountsType entity)
        {
            onUpdate();
            context.ChartOfAccountsTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ChartOfAccountsType> All()
        {
            return context.ChartOfAccountsTypes.ToList();
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
	 