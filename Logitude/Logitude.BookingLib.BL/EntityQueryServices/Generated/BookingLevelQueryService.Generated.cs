 
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
   public partial class BookingLevelQueryService: EntityQueryService<BookingLevel,BookingLevelKeys,BookingLevelPM,object,BookingLevelKeys>
   {
   
        BookingLevelRepository repository;
		IBookingContext  context;
        public BookingLevelQueryService(int tenant)
        {
		    context = BookingContext.GetContext(tenant);
            MainContext = context;
            repository = new BookingLevelRepository(context);
            Repository = repository;
            mapping = new BookingLevelDataMapping();
        }

        public BookingLevelQueryService(BookingLevelRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BookingLevelDataMapping();
        }

        public BookingLevelQueryService(IBookingContext context)
        {
            this.repository = new BookingLevelRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BookingLevelDataMapping();
        }
		 
		public  BookingLevelPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BookingLevelKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BookingLevel entityPOCO)
        {
            BookingLevelKeys entityKeys = new BookingLevelKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 