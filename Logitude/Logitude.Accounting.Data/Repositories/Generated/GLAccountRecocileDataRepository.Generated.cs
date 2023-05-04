 
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
   public partial class GLAccountRecocileDataRepository:IRepository<GLAccountRecocileData>
   {
   
        private IAccountingContext currentContext;
        public GLAccountRecocileDataRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountRecocileDataRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  GLAccountRecocileData GetSingle(string accountid, int tenant)
        {
            return (from a in context.GLAccountRecocileDatas
                    where a.AccountId == accountid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GLAccountRecocileData> GetAll(int tenant)
        {
            return from a in context.GLAccountRecocileDatas  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GLAccountRecocileData GetSingle(EntityKeyFields entityKeys)
        {
            GLAccountRecocileDataKeys keys = entityKeys as GLAccountRecocileDataKeys;
            return (from a in context.GLAccountRecocileDatas
                    where a.AccountId == keys.AccountId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GLAccountRecocileData entity)
        {
            onAdd();
            context.GLAccountRecocileDatas.Add(entity);
        }

        public void Remove(GLAccountRecocileData entity)
        {
            context.GLAccountRecocileDatas.Attach(entity);
            context.GLAccountRecocileDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GLAccountRecocileData entity)
        {
            onUpdate();
            context.GLAccountRecocileDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GLAccountRecocileData> All()
        {
            return context.GLAccountRecocileDatas.ToList();
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
	 