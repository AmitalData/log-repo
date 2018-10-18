 
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
   public partial class BookingAnswerQueryService: EntityQueryService<BookingAnswer,BookingAnswerKeys,BookingAnswerPM,BookingPM,BookingKeys>
   {
   
        BookingAnswerRepository repository;
		IBookingContext  context;
        public BookingAnswerQueryService(int tenant)
        {
		    context = BookingContext.GetContext(tenant);
            MainContext = context;
            repository = new BookingAnswerRepository(context);
            Repository = repository;
            mapping = new BookingAnswerDataMapping();
        }

        public BookingAnswerQueryService(BookingAnswerRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BookingAnswerDataMapping();
        }

        public BookingAnswerQueryService(IBookingContext context)
        {
            this.repository = new BookingAnswerRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BookingAnswerDataMapping();
        }
		 
		public  BookingAnswerPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BookingAnswerKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BookingAnswer entityPOCO)
        {
            BookingAnswerKeys entityKeys = new BookingAnswerKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 