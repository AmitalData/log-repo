 
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
   public partial class BookingLastRequestQueryService: EntityQueryService<BookingLastRequest,BookingLastRequestKeys,BookingLastRequestPM,BookingPM,BookingKeys>
   {
   
        BookingLastRequestRepository repository;
		IBookingContext  context;
        public BookingLastRequestQueryService(int tenant)
        {
		    context = BookingContext.GetContext(tenant);
            MainContext = context;
            repository = new BookingLastRequestRepository(context);
            Repository = repository;
            mapping = new BookingLastRequestDataMapping();
        }

        public BookingLastRequestQueryService(BookingLastRequestRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BookingLastRequestDataMapping();
        }

        public BookingLastRequestQueryService(IBookingContext context)
        {
            this.repository = new BookingLastRequestRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BookingLastRequestDataMapping();
        }
		 
		public  BookingLastRequestPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BookingLastRequestKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BookingLastRequest entityPOCO)
        {
            BookingLastRequestKeys entityKeys = new BookingLastRequestKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 