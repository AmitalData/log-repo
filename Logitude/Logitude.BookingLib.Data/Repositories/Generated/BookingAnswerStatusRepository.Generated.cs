 
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
   public partial class BookingAnswerStatusRepository:IRepository<BookingAnswerStatus>
   {
   
        private IBookingContext currentContext;
        public BookingAnswerStatusRepository(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public BookingAnswerStatusRepository(IBookingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BookingAnswerStatus GetSingle(string code)
        {
            return (from a in context.BookingAnswerStatus
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<BookingAnswerStatus> GetAll()
        {
            return from a in context.BookingAnswerStatus  
                   select a;
        }
				 
        public BookingAnswerStatus GetSingle(EntityKeyFields entityKeys)
        {
            BookingAnswerStatusKeys keys = entityKeys as BookingAnswerStatusKeys;
            return (from a in context.BookingAnswerStatus
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BookingAnswerStatus entity)
        {
            onAdd();
            context.BookingAnswerStatus.Add(entity);
        }

        public void Remove(BookingAnswerStatus entity)
        {
            context.BookingAnswerStatus.Attach(entity);
            context.BookingAnswerStatus.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BookingAnswerStatus entity)
        {
            onUpdate();
            context.BookingAnswerStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BookingAnswerStatus> All()
        {
            return context.BookingAnswerStatus.ToList();
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
	 