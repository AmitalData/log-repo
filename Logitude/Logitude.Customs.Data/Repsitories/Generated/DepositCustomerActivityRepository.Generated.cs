 
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
   public partial class DepositCustomerActivityRepository:IRepository<DepositCustomerActivity>
   {
   
        private ICustomContext currentContext;
        public DepositCustomerActivityRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DepositCustomerActivityRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DepositCustomerActivity GetSingle(string code)
        {
            return (from a in context.DepositCustomerActivities
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DepositCustomerActivity> GetAll()
        {
            return from a in context.DepositCustomerActivities  
                   select a;
        }
				 
        public DepositCustomerActivity GetSingle(EntityKeyFields entityKeys)
        {
            DepositCustomerActivityKeys keys = entityKeys as DepositCustomerActivityKeys;
            return (from a in context.DepositCustomerActivities
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DepositCustomerActivity entity)
        {
            onAdd();
            context.DepositCustomerActivities.Add(entity);
        }

        public void Remove(DepositCustomerActivity entity)
        {
            context.DepositCustomerActivities.Attach(entity);
            context.DepositCustomerActivities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DepositCustomerActivity entity)
        {
            onUpdate();
            context.DepositCustomerActivities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DepositCustomerActivity> All()
        {
            return context.DepositCustomerActivities.ToList();
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
	 