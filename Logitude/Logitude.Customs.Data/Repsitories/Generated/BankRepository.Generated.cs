 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class BankRepository:IRepository<Bank>
   {
   
        private ICustomContext currentContext;
        public BankRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public BankRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  Bank GetSingle(string code)
        {
            return (from a in context.Banks
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<Bank> GetAll()
        {
            return from a in context.Banks  
                   select a;
        }
				 
        public Bank GetSingle(EntityKeyFields entityKeys)
        {
            BankKeys keys = entityKeys as BankKeys;
            return (from a in context.Banks
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Bank entity)
        {
            onAdd();
            context.Banks.Add(entity);
        }

        public void Remove(Bank entity)
        {
            context.Banks.Attach(entity);
            context.Banks.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Bank entity)
        {
            onUpdate();
            context.Banks.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Bank> All()
        {
            return context.Banks.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 