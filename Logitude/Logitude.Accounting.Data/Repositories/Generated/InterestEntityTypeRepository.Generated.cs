 
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
   public partial class InterestEntityTypeRepository:IRepository<InterestEntityType>
   {
   
        private IAccountingContext currentContext;
        public InterestEntityTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public InterestEntityTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  InterestEntityType GetSingle(string code)
        {
            return (from a in context.InterestEntityTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<InterestEntityType> GetAll()
        {
            return from a in context.InterestEntityTypes  
                   select a;
        }
				 
        public InterestEntityType GetSingle(EntityKeyFields entityKeys)
        {
            InterestEntityTypeKeys keys = entityKeys as InterestEntityTypeKeys;
            return (from a in context.InterestEntityTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InterestEntityType entity)
        {
            onAdd();
            context.InterestEntityTypes.Add(entity);
        }

        public void Remove(InterestEntityType entity)
        {
            context.InterestEntityTypes.Attach(entity);
            context.InterestEntityTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InterestEntityType entity)
        {
            onUpdate();
            context.InterestEntityTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InterestEntityType> All()
        {
            return context.InterestEntityTypes.ToList();
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
	 