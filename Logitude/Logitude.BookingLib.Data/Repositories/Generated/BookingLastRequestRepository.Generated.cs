 
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
   public partial class BookingLastRequestRepository:IRepository<BookingLastRequest>
   {
   
        private IBookingContext currentContext;
        public BookingLastRequestRepository(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public BookingLastRequestRepository(IBookingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BookingLastRequest GetSingle(string id, int tenant)
        {
            return (from a in context.BookingLastRequests
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BookingLastRequest> GetAll(int tenant)
        {
            return from a in context.BookingLastRequests  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BookingLastRequest GetSingle(EntityKeyFields entityKeys)
        {
            BookingLastRequestKeys keys = entityKeys as BookingLastRequestKeys;
            return (from a in context.BookingLastRequests
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BookingLastRequest entity)
        {
            onAdd();
            context.BookingLastRequests.Add(entity);
        }

        public void Remove(BookingLastRequest entity)
        {
            context.BookingLastRequests.Attach(entity);
            context.BookingLastRequests.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BookingLastRequest entity)
        {
            onUpdate();
            context.BookingLastRequests.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BookingLastRequest> All()
        {
            return context.BookingLastRequests.ToList();
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
	 