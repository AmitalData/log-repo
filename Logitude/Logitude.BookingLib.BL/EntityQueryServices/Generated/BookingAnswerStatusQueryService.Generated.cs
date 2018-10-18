 
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
   public partial class BookingAnswerStatusQueryService: EntityQueryService<BookingAnswerStatus,BookingAnswerStatusKeys,BookingAnswerStatusPM,object,BookingAnswerStatusKeys>
   {
   
        BookingAnswerStatusRepository repository;
		IBookingContext  context;
        public BookingAnswerStatusQueryService(int tenant)
        {
		    context = BookingContext.GetContext(tenant);
            MainContext = context;
            repository = new BookingAnswerStatusRepository(context);
            Repository = repository;
            mapping = new BookingAnswerStatusDataMapping();
        }

        public BookingAnswerStatusQueryService(BookingAnswerStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BookingAnswerStatusDataMapping();
        }

        public BookingAnswerStatusQueryService(IBookingContext context)
        {
            this.repository = new BookingAnswerStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BookingAnswerStatusDataMapping();
        }
		 
		public  BookingAnswerStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BookingAnswerStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BookingAnswerStatus entityPOCO)
        {
            BookingAnswerStatusKeys entityKeys = new BookingAnswerStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 