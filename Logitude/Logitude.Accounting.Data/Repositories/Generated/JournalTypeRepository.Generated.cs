 
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
   public partial class JournalTypeRepository:IRepository<JournalType>
   {
   
        private IAccountingContext currentContext;
        public JournalTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public JournalTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  JournalType GetSingle(string journaltypeid)
        {
            return (from a in context.JournalTypes
                    where a.JournalTypeID == journaltypeid 
                    select a).FirstOrDefault();
        }

        public IQueryable<JournalType> GetAll()
        {
            return from a in context.JournalTypes  
                   select a;
        }
				 
        public JournalType GetSingle(EntityKeyFields entityKeys)
        {
            JournalTypeKeys keys = entityKeys as JournalTypeKeys;
            return (from a in context.JournalTypes
                    where a.JournalTypeID == keys.JournalTypeID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(JournalType entity)
        {
            onAdd();
            context.JournalTypes.Add(entity);
        }

        public void Remove(JournalType entity)
        {
            context.JournalTypes.Attach(entity);
            context.JournalTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(JournalType entity)
        {
            onUpdate();
            context.JournalTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<JournalType> All()
        {
            return context.JournalTypes.ToList();
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
	 