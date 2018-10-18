 
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
   public partial class BookingQueryService: EntityQueryService<Booking,BookingKeys,BookingPM,object,BookingKeys>
   {
   
        BookingRepository repository;
		IBookingContext  context;
        public BookingQueryService(int tenant)
        {
		    context = BookingContext.GetContext(tenant);
            MainContext = context;
            repository = new BookingRepository(context);
            Repository = repository;
            mapping = new BookingDataMapping();
        }

        public BookingQueryService(BookingRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BookingDataMapping();
        }

        public BookingQueryService(IBookingContext context)
        {
            this.repository = new BookingRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BookingDataMapping();
        }
		 
		public  BookingPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BookingKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Booking entityPOCO)
        {
            BookingKeys entityKeys = new BookingKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 