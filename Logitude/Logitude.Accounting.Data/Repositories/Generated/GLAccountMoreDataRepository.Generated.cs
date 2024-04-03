 
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
   public partial class GLAccountMoreDataRepository:IRepository<GLAccountMoreData>
   {
   
        private IAccountingContext currentContext;
        public GLAccountMoreDataRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountMoreDataRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  GLAccountMoreData GetSingle(string accountid, int tenant)
        {
            return (from a in context.GLAccountMoreDatas
                    where a.AccountId == accountid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GLAccountMoreData> GetAll(int tenant)
        {
            return from a in context.GLAccountMoreDatas  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GLAccountMoreData GetSingle(EntityKeyFields entityKeys)
        {
            GLAccountMoreDataKeys keys = entityKeys as GLAccountMoreDataKeys;
            return (from a in context.GLAccountMoreDatas
                    where a.AccountId == keys.AccountId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GLAccountMoreData entity)
        {
            onAdd();
            context.GLAccountMoreDatas.Add(entity);
        }

        public void Remove(GLAccountMoreData entity)
        {
            context.GLAccountMoreDatas.Attach(entity);
            context.GLAccountMoreDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GLAccountMoreData entity)
        {
            onUpdate();
            context.GLAccountMoreDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GLAccountMoreData> All()
        {
            return context.GLAccountMoreDatas.ToList();
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
	 