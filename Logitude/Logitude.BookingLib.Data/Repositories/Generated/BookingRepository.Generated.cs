 
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
   public partial class BookingRepository:IRepository<Booking>
   {
   
        private IBookingContext currentContext;
        public BookingRepository(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public BookingRepository(IBookingContext context)
        {
            currentContext = context;
        }

		 
		
		public  Booking GetSingle(string id, int tenant)
        {
            return (from a in context.Bookings
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Booking> GetAll(int tenant)
        {
            return from a in context.Bookings  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Booking GetSingle(EntityKeyFields entityKeys)
        {
            BookingKeys keys = entityKeys as BookingKeys;
            return (from a in context.Bookings
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Booking entity)
        {
            onAdd();
            context.Bookings.Add(entity);
        }

        public void Remove(Booking entity)
        {
            context.Bookings.Attach(entity);
            context.Bookings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Booking entity)
        {
            onUpdate();
            context.Bookings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Booking> All()
        {
            return context.Bookings.ToList();
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
	 