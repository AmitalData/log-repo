 
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
   public partial class FlightsSchedulesResponseRepository:IRepository<FlightsSchedulesResponse>
   {
   
        private IBookingContext currentContext;
        public FlightsSchedulesResponseRepository(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public FlightsSchedulesResponseRepository(IBookingContext context)
        {
            currentContext = context;
        }

		 
		
		public  FlightsSchedulesResponse GetSingle(string id, int tenant)
        {
            return (from a in context.FlightsSchedulesResponses
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<FlightsSchedulesResponse> GetAll(int tenant)
        {
            return from a in context.FlightsSchedulesResponses  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public FlightsSchedulesResponse GetSingle(EntityKeyFields entityKeys)
        {
            FlightsSchedulesResponseKeys keys = entityKeys as FlightsSchedulesResponseKeys;
            return (from a in context.FlightsSchedulesResponses
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(FlightsSchedulesResponse entity)
        {
            onAdd();
            context.FlightsSchedulesResponses.Add(entity);
        }

        public void Remove(FlightsSchedulesResponse entity)
        {
            context.FlightsSchedulesResponses.Attach(entity);
            context.FlightsSchedulesResponses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(FlightsSchedulesResponse entity)
        {
            onUpdate();
            context.FlightsSchedulesResponses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FlightsSchedulesResponse> All()
        {
            return context.FlightsSchedulesResponses.ToList();
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
	 