 
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
   public partial class FlightsSchedulesRequestRepository:IRepository<FlightsSchedulesRequest>
   {
   
        private IBookingContext currentContext;
        public FlightsSchedulesRequestRepository(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public FlightsSchedulesRequestRepository(IBookingContext context)
        {
            currentContext = context;
        }

		 
		
		public  FlightsSchedulesRequest GetSingle(string id, int tenant)
        {
            return (from a in context.FlightsSchedulesRequests
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<FlightsSchedulesRequest> GetAll(int tenant)
        {
            return from a in context.FlightsSchedulesRequests  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public FlightsSchedulesRequest GetSingle(EntityKeyFields entityKeys)
        {
            FlightsSchedulesRequestKeys keys = entityKeys as FlightsSchedulesRequestKeys;
            return (from a in context.FlightsSchedulesRequests
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(FlightsSchedulesRequest entity)
        {
            onAdd();
            context.FlightsSchedulesRequests.Add(entity);
        }

        public void Remove(FlightsSchedulesRequest entity)
        {
            context.FlightsSchedulesRequests.Attach(entity);
            context.FlightsSchedulesRequests.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(FlightsSchedulesRequest entity)
        {
            onUpdate();
            context.FlightsSchedulesRequests.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FlightsSchedulesRequest> All()
        {
            return context.FlightsSchedulesRequests.ToList();
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
	 