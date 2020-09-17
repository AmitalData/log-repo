 
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
   public partial class CourierPendingReasonRepository:IRepository<CourierPendingReason>
   {
   
        private ICustomContext currentContext;
        public CourierPendingReasonRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CourierPendingReasonRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CourierPendingReason GetSingle(string id, int tenant)
        {
            return (from a in context.CourierPendingReasons
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
        public CourierPendingReason GetByCode(string code, int tenant)
        {
            return (from a in context.CourierPendingReasons
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CourierPendingReason> GetAll(int tenant)
        {
            return from a in context.CourierPendingReasons  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CourierPendingReason GetSingle(EntityKeyFields entityKeys)
        {
            CourierPendingReasonKeys keys = entityKeys as CourierPendingReasonKeys;
            return (from a in context.CourierPendingReasons
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CourierPendingReason entity)
        {
            onAdd();
            context.CourierPendingReasons.Add(entity);
        }

        public void Remove(CourierPendingReason entity)
        {
            context.CourierPendingReasons.Attach(entity);
            context.CourierPendingReasons.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CourierPendingReason entity)
        {
            onUpdate();
            context.CourierPendingReasons.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CourierPendingReason> All()
        {
            return context.CourierPendingReasons.ToList();
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
	 