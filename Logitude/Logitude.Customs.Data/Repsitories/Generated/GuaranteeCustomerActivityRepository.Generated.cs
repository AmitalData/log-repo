 
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
   public partial class GuaranteeCustomerActivityRepository:IRepository<GuaranteeCustomerActivity>
   {
   
        private ICustomContext currentContext;
        public GuaranteeCustomerActivityRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public GuaranteeCustomerActivityRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  GuaranteeCustomerActivity GetSingle(string code)
        {
            return (from a in context.GuaranteeCustomerActivities
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<GuaranteeCustomerActivity> GetAll()
        {
            return from a in context.GuaranteeCustomerActivities  
                   select a;
        }
				 
        public GuaranteeCustomerActivity GetSingle(EntityKeyFields entityKeys)
        {
            GuaranteeCustomerActivityKeys keys = entityKeys as GuaranteeCustomerActivityKeys;
            return (from a in context.GuaranteeCustomerActivities
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GuaranteeCustomerActivity entity)
        {
            onAdd();
            context.GuaranteeCustomerActivities.Add(entity);
        }

        public void Remove(GuaranteeCustomerActivity entity)
        {
            context.GuaranteeCustomerActivities.Attach(entity);
            context.GuaranteeCustomerActivities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GuaranteeCustomerActivity entity)
        {
            onUpdate();
            context.GuaranteeCustomerActivities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GuaranteeCustomerActivity> All()
        {
            return context.GuaranteeCustomerActivities.ToList();
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
	 