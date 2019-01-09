 
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
   public partial class RefundCustomerActivityTypeRepository:IRepository<RefundCustomerActivityType>
   {
   
        private ICustomContext currentContext;
        public RefundCustomerActivityTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RefundCustomerActivityTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RefundCustomerActivityType GetSingle(string code)
        {
            return (from a in context.RefundCustomerActivityTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RefundCustomerActivityType> GetAll()
        {
            return from a in context.RefundCustomerActivityTypes  
                   select a;
        }
				 
        public RefundCustomerActivityType GetSingle(EntityKeyFields entityKeys)
        {
            RefundCustomerActivityTypeKeys keys = entityKeys as RefundCustomerActivityTypeKeys;
            return (from a in context.RefundCustomerActivityTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RefundCustomerActivityType entity)
        {
            onAdd();
            context.RefundCustomerActivityTypes.Add(entity);
        }

        public void Remove(RefundCustomerActivityType entity)
        {
            context.RefundCustomerActivityTypes.Attach(entity);
            context.RefundCustomerActivityTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RefundCustomerActivityType entity)
        {
            onUpdate();
            context.RefundCustomerActivityTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RefundCustomerActivityType> All()
        {
            return context.RefundCustomerActivityTypes.ToList();
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
	 