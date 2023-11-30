 
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
   public partial class GLAccountCardsDataRepository:IRepository<GLAccountCardsData>
   {
   
        private IAccountingContext currentContext;
        public GLAccountCardsDataRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountCardsDataRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  GLAccountCardsData GetSingle(string id, int tenant)
        {
            return (from a in context.GLAccountCardsDatas
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GLAccountCardsData> GetAll(int tenant)
        {
            return from a in context.GLAccountCardsDatas  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GLAccountCardsData GetSingle(EntityKeyFields entityKeys)
        {
            GLAccountCardsDataKeys keys = entityKeys as GLAccountCardsDataKeys;
            return (from a in context.GLAccountCardsDatas
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GLAccountCardsData entity)
        {
            onAdd();
            context.GLAccountCardsDatas.Add(entity);
        }

        public void Remove(GLAccountCardsData entity)
        {
            context.GLAccountCardsDatas.Attach(entity);
            context.GLAccountCardsDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GLAccountCardsData entity)
        {
            onUpdate();
            context.GLAccountCardsDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GLAccountCardsData> All()
        {
            return context.GLAccountCardsDatas.ToList();
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
	 