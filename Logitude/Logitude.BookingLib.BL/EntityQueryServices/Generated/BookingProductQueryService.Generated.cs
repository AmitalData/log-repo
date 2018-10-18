 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.BL.EntityDataMappings;
using Logitude.BookingLib.Data.Repositories;
using Logitude.BookingLib.Data.EntityKeys;
using Logitude.BookingLib.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.BookingLib.BL.EntityQueryServices
{ 
   public partial class BookingProductQueryService: EntityQueryService<BookingProduct,BookingProductKeys,BookingProductPM,object,BookingProductKeys>
   {
   
        BookingProductRepository repository;
		IBookingContext  context;
        public BookingProductQueryService(int tenant)
        {
		    context = BookingContext.GetContext(tenant);
            MainContext = context;
            repository = new BookingProductRepository(context);
            Repository = repository;
            mapping = new BookingProductDataMapping();
        }

        public BookingProductQueryService(BookingProductRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BookingProductDataMapping();
        }

        public BookingProductQueryService(IBookingContext context)
        {
            this.repository = new BookingProductRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BookingProductDataMapping();
        }
		 
		public  BookingProductPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BookingProductKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BookingProduct entityPOCO)
        {
            BookingProductKeys entityKeys = new BookingProductKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 