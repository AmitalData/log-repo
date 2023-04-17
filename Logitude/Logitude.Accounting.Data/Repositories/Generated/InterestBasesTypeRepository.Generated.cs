 
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
   public partial class InterestBasesTypeRepository:IRepository<InterestBasesType>
   {
   
        private IAccountingContext currentContext;
        public InterestBasesTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public InterestBasesTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  InterestBasesType GetSingle(string id, int tenant)
        {
            return (from a in context.InterestBasesTypes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<InterestBasesType> GetAll(int tenant)
        {
            return from a in context.InterestBasesTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public InterestBasesType GetSingle(EntityKeyFields entityKeys)
        {
            InterestBasesTypeKeys keys = entityKeys as InterestBasesTypeKeys;
            return (from a in context.InterestBasesTypes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InterestBasesType entity)
        {
            onAdd();
            context.InterestBasesTypes.Add(entity);
        }

        public void Remove(InterestBasesType entity)
        {
            context.InterestBasesTypes.Attach(entity);
            context.InterestBasesTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InterestBasesType entity)
        {
            onUpdate();
            context.InterestBasesTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InterestBasesType> All()
        {
            return context.InterestBasesTypes.ToList();
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
	 