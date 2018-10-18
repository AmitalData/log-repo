 
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
   public partial class BookingStatusRepository:IRepository<BookingStatus>
   {
   
        private IBookingContext currentContext;
        public BookingStatusRepository(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public BookingStatusRepository(IBookingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BookingStatus GetSingle(string code)
        {
            return (from a in context.BookingStatus
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<BookingStatus> GetAll()
        {
            return from a in context.BookingStatus  
                   select a;
        }
				 
        public BookingStatus GetSingle(EntityKeyFields entityKeys)
        {
            BookingStatusKeys keys = entityKeys as BookingStatusKeys;
            return (from a in context.BookingStatus
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BookingStatus entity)
        {
            onAdd();
            context.BookingStatus.Add(entity);
        }

        public void Remove(BookingStatus entity)
        {
            context.BookingStatus.Attach(entity);
            context.BookingStatus.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BookingStatus entity)
        {
            onUpdate();
            context.BookingStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BookingStatus> All()
        {
            return context.BookingStatus.ToList();
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
	 