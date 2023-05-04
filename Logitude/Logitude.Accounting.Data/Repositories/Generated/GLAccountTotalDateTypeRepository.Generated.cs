 
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
   public partial class GLAccountTotalDateTypeRepository:IRepository<GLAccountTotalDateType>
   {
   
        private IAccountingContext currentContext;
        public GLAccountTotalDateTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountTotalDateTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  GLAccountTotalDateType GetSingle(string code)
        {
            return (from a in context.GLAccountTotalDateTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<GLAccountTotalDateType> GetAll()
        {
            return from a in context.GLAccountTotalDateTypes  
                   select a;
        }
				 
        public GLAccountTotalDateType GetSingle(EntityKeyFields entityKeys)
        {
            GLAccountTotalDateTypeKeys keys = entityKeys as GLAccountTotalDateTypeKeys;
            return (from a in context.GLAccountTotalDateTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GLAccountTotalDateType entity)
        {
            onAdd();
            context.GLAccountTotalDateTypes.Add(entity);
        }

        public void Remove(GLAccountTotalDateType entity)
        {
            context.GLAccountTotalDateTypes.Attach(entity);
            context.GLAccountTotalDateTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GLAccountTotalDateType entity)
        {
            onUpdate();
            context.GLAccountTotalDateTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GLAccountTotalDateType> All()
        {
            return context.GLAccountTotalDateTypes.ToList();
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
	 