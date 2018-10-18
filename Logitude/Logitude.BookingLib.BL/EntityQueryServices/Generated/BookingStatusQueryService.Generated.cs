 
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
   public partial class BookingStatusQueryService: EntityQueryService<BookingStatus,BookingStatusKeys,BookingStatusPM,object,BookingStatusKeys>
   {
   
        BookingStatusRepository repository;
		IBookingContext  context;
        public BookingStatusQueryService(int tenant)
        {
		    context = BookingContext.GetContext(tenant);
            MainContext = context;
            repository = new BookingStatusRepository(context);
            Repository = repository;
            mapping = new BookingStatusDataMapping();
        }

        public BookingStatusQueryService(BookingStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BookingStatusDataMapping();
        }

        public BookingStatusQueryService(IBookingContext context)
        {
            this.repository = new BookingStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BookingStatusDataMapping();
        }
		 
		public  BookingStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BookingStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BookingStatus entityPOCO)
        {
            BookingStatusKeys entityKeys = new BookingStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 