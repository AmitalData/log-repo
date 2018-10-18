 
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
   public partial class BookingPackageQueryService: EntityQueryService<BookingPackage,BookingPackageKeys,BookingPackagePM,BookingPM,BookingKeys>
   {
   
        BookingPackageRepository repository;
		IBookingContext  context;
        public BookingPackageQueryService(int tenant)
        {
		    context = BookingContext.GetContext(tenant);
            MainContext = context;
            repository = new BookingPackageRepository(context);
            Repository = repository;
            mapping = new BookingPackageDataMapping();
        }

        public BookingPackageQueryService(BookingPackageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BookingPackageDataMapping();
        }

        public BookingPackageQueryService(IBookingContext context)
        {
            this.repository = new BookingPackageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BookingPackageDataMapping();
        }
		 
		public  BookingPackagePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BookingPackageKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BookingPackage entityPOCO)
        {
            BookingPackageKeys entityKeys = new BookingPackageKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 