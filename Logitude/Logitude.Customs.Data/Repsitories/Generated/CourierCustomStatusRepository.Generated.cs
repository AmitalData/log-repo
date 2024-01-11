 
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
   public partial class CourierCustomStatusRepository:IRepository<CourierCustomStatus>
   {
   
        private ICustomContext currentContext;
        public CourierCustomStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CourierCustomStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CourierCustomStatus GetSingle(string code)
        {
            return (from a in context.CourierCustomStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CourierCustomStatus> GetAll()
        {
            return from a in context.CourierCustomStatuses  
                   select a;
        }
				 
        public CourierCustomStatus GetSingle(EntityKeyFields entityKeys)
        {
            CourierCustomStatusKeys keys = entityKeys as CourierCustomStatusKeys;
            return (from a in context.CourierCustomStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CourierCustomStatus entity)
        {
            onAdd();
            context.CourierCustomStatuses.Add(entity);
        }

        public void Remove(CourierCustomStatus entity)
        {
            context.CourierCustomStatuses.Attach(entity);
            context.CourierCustomStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CourierCustomStatus entity)
        {
            onUpdate();
            context.CourierCustomStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CourierCustomStatus> All()
        {
            return context.CourierCustomStatuses.ToList();
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
	 