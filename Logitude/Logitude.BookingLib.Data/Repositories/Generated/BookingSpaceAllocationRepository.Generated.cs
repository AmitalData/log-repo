 
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
   public partial class BookingSpaceAllocationRepository:IRepository<BookingSpaceAllocation>
   {
   
        private IBookingContext currentContext;
        public BookingSpaceAllocationRepository(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public BookingSpaceAllocationRepository(IBookingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BookingSpaceAllocation GetSingle(string code)
        {
            return (from a in context.BookingSpaceAllocations
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<BookingSpaceAllocation> GetAll()
        {
            return from a in context.BookingSpaceAllocations  
                   select a;
        }
				 
        public BookingSpaceAllocation GetSingle(EntityKeyFields entityKeys)
        {
            BookingSpaceAllocationKeys keys = entityKeys as BookingSpaceAllocationKeys;
            return (from a in context.BookingSpaceAllocations
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BookingSpaceAllocation entity)
        {
            onAdd();
            context.BookingSpaceAllocations.Add(entity);
        }

        public void Remove(BookingSpaceAllocation entity)
        {
            context.BookingSpaceAllocations.Attach(entity);
            context.BookingSpaceAllocations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BookingSpaceAllocation entity)
        {
            onUpdate();
            context.BookingSpaceAllocations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BookingSpaceAllocation> All()
        {
            return context.BookingSpaceAllocations.ToList();
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
	 