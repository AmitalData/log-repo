 
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
   public partial class CourierStatusRepository:IRepository<CourierStatus>
   {
   
        private ICustomContext currentContext;
        public CourierStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CourierStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CourierStatus GetSingle(string code)
        {
            return (from a in context.CourierStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CourierStatus> GetAll()
        {
            return from a in context.CourierStatuses  
                   select a;
        }
				 
        public CourierStatus GetSingle(EntityKeyFields entityKeys)
        {
            CourierStatusKeys keys = entityKeys as CourierStatusKeys;
            return (from a in context.CourierStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CourierStatus entity)
        {
            onAdd();
            context.CourierStatuses.Add(entity);
        }

        public void Remove(CourierStatus entity)
        {
            context.CourierStatuses.Attach(entity);
            context.CourierStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CourierStatus entity)
        {
            onUpdate();
            context.CourierStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CourierStatus> All()
        {
            return context.CourierStatuses.ToList();
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
	 