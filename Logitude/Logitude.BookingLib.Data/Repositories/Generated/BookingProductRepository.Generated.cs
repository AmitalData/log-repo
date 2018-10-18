 
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
   public partial class BookingProductRepository:IRepository<BookingProduct>
   {
   
        private IBookingContext currentContext;
        public BookingProductRepository(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public BookingProductRepository(IBookingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BookingProduct GetSingle(string id)
        {
            return (from a in context.BookingProducts
                    where a.Id == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<BookingProduct> GetAll()
        {
            return from a in context.BookingProducts  
                   select a;
        }
				 
        public BookingProduct GetSingle(EntityKeyFields entityKeys)
        {
            BookingProductKeys keys = entityKeys as BookingProductKeys;
            return (from a in context.BookingProducts
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BookingProduct entity)
        {
            onAdd();
            context.BookingProducts.Add(entity);
        }

        public void Remove(BookingProduct entity)
        {
            context.BookingProducts.Attach(entity);
            context.BookingProducts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BookingProduct entity)
        {
            onUpdate();
            context.BookingProducts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BookingProduct> All()
        {
            return context.BookingProducts.ToList();
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
	 