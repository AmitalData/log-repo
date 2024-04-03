 
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
   public partial class DepositRepository:IRepository<Deposit>
   {
   
        private ICustomContext currentContext;
        public DepositRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DepositRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  Deposit GetSingle(string id, int tenant)
        {
            return (from a in context.Deposits
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Deposit> GetAll(int tenant)
        {
            return from a in context.Deposits  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Deposit GetSingle(EntityKeyFields entityKeys)
        {
            DepositKeys keys = entityKeys as DepositKeys;
            return (from a in context.Deposits
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Deposit entity)
        {
            onAdd();
            context.Deposits.Add(entity);
        }

        public void Remove(Deposit entity)
        {
            context.Deposits.Attach(entity);
            context.Deposits.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Deposit entity)
        {
            onUpdate();
            context.Deposits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Deposit> All()
        {
            return context.Deposits.ToList();
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
	 