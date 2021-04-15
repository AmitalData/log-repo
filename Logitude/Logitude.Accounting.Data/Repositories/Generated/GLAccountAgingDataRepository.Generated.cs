 
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
   public partial class GLAccountAgingDataRepository:IRepository<GLAccountAgingData>
   {
   
        private IAccountingContext currentContext;
        public GLAccountAgingDataRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountAgingDataRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  GLAccountAgingData GetSingle(string accountid, int tenant)
        {
            return (from a in context.GLAccountAgingDatas
                    where a.AccountId == accountid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GLAccountAgingData> GetAll(int tenant)
        {
            return from a in context.GLAccountAgingDatas  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GLAccountAgingData GetSingle(EntityKeyFields entityKeys)
        {
            GLAccountAgingDataKeys keys = entityKeys as GLAccountAgingDataKeys;
            return (from a in context.GLAccountAgingDatas
                    where a.AccountId == keys.AccountId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GLAccountAgingData entity)
        {
            onAdd();
            context.GLAccountAgingDatas.Add(entity);
        }

        public void Remove(GLAccountAgingData entity)
        {
            context.GLAccountAgingDatas.Attach(entity);
            context.GLAccountAgingDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GLAccountAgingData entity)
        {
            onUpdate();
            context.GLAccountAgingDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GLAccountAgingData> All()
        {
            return context.GLAccountAgingDatas.ToList();
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
	 