 
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
   public partial class BookingSpaceAllocationQueryService: EntityQueryService<BookingSpaceAllocation,BookingSpaceAllocationKeys,BookingSpaceAllocationPM,object,BookingSpaceAllocationKeys>
   {
   
        BookingSpaceAllocationRepository repository;
		IBookingContext  context;
        public BookingSpaceAllocationQueryService(int tenant)
        {
		    context = BookingContext.GetContext(tenant);
            MainContext = context;
            repository = new BookingSpaceAllocationRepository(context);
            Repository = repository;
            mapping = new BookingSpaceAllocationDataMapping();
        }

        public BookingSpaceAllocationQueryService(BookingSpaceAllocationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BookingSpaceAllocationDataMapping();
        }

        public BookingSpaceAllocationQueryService(IBookingContext context)
        {
            this.repository = new BookingSpaceAllocationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BookingSpaceAllocationDataMapping();
        }
		 
		public  BookingSpaceAllocationPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BookingSpaceAllocationKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BookingSpaceAllocation entityPOCO)
        {
            BookingSpaceAllocationKeys entityKeys = new BookingSpaceAllocationKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 