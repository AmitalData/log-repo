 
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
   public partial class GLAccountFollowUpDataRepository:IRepository<GLAccountFollowUpData>
   {
   
        private IAccountingContext currentContext;
        public GLAccountFollowUpDataRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountFollowUpDataRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  GLAccountFollowUpData GetSingle(string id, int tenant)
        {
            return (from a in context.GLAccountFollowUpDatas
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GLAccountFollowUpData> GetAll(int tenant)
        {
            return from a in context.GLAccountFollowUpDatas  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GLAccountFollowUpData GetSingle(EntityKeyFields entityKeys)
        {
            GLAccountFollowUpDataKeys keys = entityKeys as GLAccountFollowUpDataKeys;
            return (from a in context.GLAccountFollowUpDatas
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GLAccountFollowUpData entity)
        {
            onAdd();
            context.GLAccountFollowUpDatas.Add(entity);
        }

        public void Remove(GLAccountFollowUpData entity)
        {
            context.GLAccountFollowUpDatas.Attach(entity);
            context.GLAccountFollowUpDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GLAccountFollowUpData entity)
        {
            onUpdate();
            context.GLAccountFollowUpDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GLAccountFollowUpData> All()
        {
            return context.GLAccountFollowUpDatas.ToList();
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
	 