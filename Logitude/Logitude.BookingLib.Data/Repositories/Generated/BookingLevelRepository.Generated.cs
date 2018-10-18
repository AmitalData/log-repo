 
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
   public partial class BookingLevelRepository:IRepository<BookingLevel>
   {
   
        private IBookingContext currentContext;
        public BookingLevelRepository(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public BookingLevelRepository(IBookingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BookingLevel GetSingle(string code)
        {
            return (from a in context.BookingLevels
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<BookingLevel> GetAll()
        {
            return from a in context.BookingLevels  
                   select a;
        }
				 
        public BookingLevel GetSingle(EntityKeyFields entityKeys)
        {
            BookingLevelKeys keys = entityKeys as BookingLevelKeys;
            return (from a in context.BookingLevels
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BookingLevel entity)
        {
            onAdd();
            context.BookingLevels.Add(entity);
        }

        public void Remove(BookingLevel entity)
        {
            context.BookingLevels.Attach(entity);
            context.BookingLevels.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BookingLevel entity)
        {
            onUpdate();
            context.BookingLevels.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BookingLevel> All()
        {
            return context.BookingLevels.ToList();
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
	 