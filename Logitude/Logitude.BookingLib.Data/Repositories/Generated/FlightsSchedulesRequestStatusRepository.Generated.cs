 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.BookingLib.Data.Repositories
{
   public partial class FlightsSchedulesRequestStatusRepository:IRepository<FlightsSchedulesRequestStatus>
   {
   
        private IBookingContext currentContext;
        public FlightsSchedulesRequestStatusRepository(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public FlightsSchedulesRequestStatusRepository(IBookingContext context)
        {
            currentContext = context;
        }

		 
		
		public  FlightsSchedulesRequestStatus GetSingle(string code)
        {
            return (from a in context.FlightsSchedulesRequestStatus
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<FlightsSchedulesRequestStatus> GetAll()
        {
            return from a in context.FlightsSchedulesRequestStatus  
                   select a;
        }
				 
        public FlightsSchedulesRequestStatus GetSingle(EntityKeyFields entityKeys)
        {
            FlightsSchedulesRequestStatusKeys keys = entityKeys as FlightsSchedulesRequestStatusKeys;
            return (from a in context.FlightsSchedulesRequestStatus
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(FlightsSchedulesRequestStatus entity)
        {
            onAdd();
            context.FlightsSchedulesRequestStatus.Add(entity);
        }

        public void Remove(FlightsSchedulesRequestStatus entity)
        {
            context.FlightsSchedulesRequestStatus.Attach(entity);
            context.FlightsSchedulesRequestStatus.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(FlightsSchedulesRequestStatus entity)
        {
            onUpdate();
            context.FlightsSchedulesRequestStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FlightsSchedulesRequestStatus> All()
        {
            return context.FlightsSchedulesRequestStatus.ToList();
        }

        private IBookingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 