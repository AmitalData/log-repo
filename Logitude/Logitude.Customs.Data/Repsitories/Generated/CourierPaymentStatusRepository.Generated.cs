 
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
   public partial class CourierPaymentStatusRepository:IRepository<CourierPaymentStatus>
   {
   
        private ICustomContext currentContext;
        public CourierPaymentStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CourierPaymentStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CourierPaymentStatus GetSingle(string code)
        {
            return (from a in context.CourierPaymentStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CourierPaymentStatus> GetAll()
        {
            return from a in context.CourierPaymentStatuses  
                   select a;
        }
				 
        public CourierPaymentStatus GetSingle(EntityKeyFields entityKeys)
        {
            CourierPaymentStatusKeys keys = entityKeys as CourierPaymentStatusKeys;
            return (from a in context.CourierPaymentStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CourierPaymentStatus entity)
        {
            onAdd();
            context.CourierPaymentStatuses.Add(entity);
        }

        public void Remove(CourierPaymentStatus entity)
        {
            context.CourierPaymentStatuses.Attach(entity);
            context.CourierPaymentStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CourierPaymentStatus entity)
        {
            onUpdate();
            context.CourierPaymentStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CourierPaymentStatus> All()
        {
            return context.CourierPaymentStatuses.ToList();
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
	 