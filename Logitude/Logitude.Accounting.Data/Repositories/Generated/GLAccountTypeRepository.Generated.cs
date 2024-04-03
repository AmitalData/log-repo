 
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
   public partial class GLAccountTypeRepository:IRepository<GLAccountType>
   {
   
        private IAccountingContext currentContext;
        public GLAccountTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  GLAccountType GetSingle(string code)
        {
            return (from a in context.GLAccountTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<GLAccountType> GetAll()
        {
            return from a in context.GLAccountTypes  
                   select a;
        }
				 
        public GLAccountType GetSingle(EntityKeyFields entityKeys)
        {
            GLAccountTypeKeys keys = entityKeys as GLAccountTypeKeys;
            return (from a in context.GLAccountTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GLAccountType entity)
        {
            onAdd();
            context.GLAccountTypes.Add(entity);
        }

        public void Remove(GLAccountType entity)
        {
            context.GLAccountTypes.Attach(entity);
            context.GLAccountTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GLAccountType entity)
        {
            onUpdate();
            context.GLAccountTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GLAccountType> All()
        {
            return context.GLAccountTypes.ToList();
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
	 