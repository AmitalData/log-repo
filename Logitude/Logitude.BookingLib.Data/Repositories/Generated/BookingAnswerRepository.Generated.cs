 
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
   public partial class BookingAnswerRepository:IRepository<BookingAnswer>
   {
   
        private IBookingContext currentContext;
        public BookingAnswerRepository(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public BookingAnswerRepository(IBookingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BookingAnswer GetSingle(string id, int tenant)
        {
            return (from a in context.BookingAnswers
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BookingAnswer> GetAll(int tenant)
        {
            return from a in context.BookingAnswers  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BookingAnswer GetSingle(EntityKeyFields entityKeys)
        {
            BookingAnswerKeys keys = entityKeys as BookingAnswerKeys;
            return (from a in context.BookingAnswers
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BookingAnswer entity)
        {
            onAdd();
            context.BookingAnswers.Add(entity);
        }

        public void Remove(BookingAnswer entity)
        {
            context.BookingAnswers.Attach(entity);
            context.BookingAnswers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BookingAnswer entity)
        {
            onUpdate();
            context.BookingAnswers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BookingAnswer> All()
        {
            return context.BookingAnswers.ToList();
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
	 