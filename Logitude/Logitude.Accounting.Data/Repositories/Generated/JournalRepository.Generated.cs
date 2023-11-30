 
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
   public partial class JournalRepository:IRepository<Journal>
   {
   
        private IAccountingContext currentContext;
        public JournalRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public JournalRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  Journal GetSingle(string id, int tenant)
        {
            return (from a in context.Journals
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Journal> GetAll(int tenant)
        {
            return from a in context.Journals  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Journal GetSingle(EntityKeyFields entityKeys)
        {
            JournalKeys keys = entityKeys as JournalKeys;
            return (from a in context.Journals
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Journal entity)
        {
            onAdd();
            context.Journals.Add(entity);
        }

        public void Remove(Journal entity)
        {
            context.Journals.Attach(entity);
            context.Journals.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Journal entity)
        {
            onUpdate();
            context.Journals.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Journal> All()
        {
            return context.Journals.ToList();
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
	 