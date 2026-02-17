 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.BL.EntityDataMappings;
using Logitude.BookingLib.Data.Repositories;
using Logitude.BookingLib.Data.EntityKeys;
using Logitude.BookingLib.Data;

namespace Logitude.BookingLib.BL.EntityUpdateServices
{ 
   public partial class BookingPackageUpdateService:EntityUpdateService<BookingPackage,BookingPackagePM,BookingPM>
   {
   
        BookingPackageRepository entityRepository;
        public BookingPackageUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IBookingContext  context = mainContext as BookingContext;
            context = context ??mainContext as IBookingContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new BookingPackageDataMapping();
            Repository = new BookingPackageRepository(context);
        }

       
        private IBookingContext currentContext;
        public BookingPackageUpdateService(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public BookingPackageUpdateService(IBookingContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(BookingPackagePM entityPM)
        {
            BookingPackageKeys entityKeys = new BookingPackageKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(BookingPackagePM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(BookingPackagePM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 