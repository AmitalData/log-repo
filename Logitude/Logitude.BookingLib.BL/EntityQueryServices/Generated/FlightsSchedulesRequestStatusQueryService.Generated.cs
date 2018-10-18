 
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
   public partial class FlightsSchedulesRequestStatusQueryService: EntityQueryService<FlightsSchedulesRequestStatus,FlightsSchedulesRequestStatusKeys,FlightsSchedulesRequestStatusPM,object,FlightsSchedulesRequestStatusKeys>
   {
   
        FlightsSchedulesRequestStatusRepository repository;
		IBookingContext  context;
        public FlightsSchedulesRequestStatusQueryService(int tenant)
        {
		    context = BookingContext.GetContext(tenant);
            MainContext = context;
            repository = new FlightsSchedulesRequestStatusRepository(context);
            Repository = repository;
            mapping = new FlightsSchedulesRequestStatusDataMapping();
        }

        public FlightsSchedulesRequestStatusQueryService(FlightsSchedulesRequestStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new FlightsSchedulesRequestStatusDataMapping();
        }

        public FlightsSchedulesRequestStatusQueryService(IBookingContext context)
        {
            this.repository = new FlightsSchedulesRequestStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new FlightsSchedulesRequestStatusDataMapping();
        }
		 
		public  FlightsSchedulesRequestStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new FlightsSchedulesRequestStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(FlightsSchedulesRequestStatus entityPOCO)
        {
            FlightsSchedulesRequestStatusKeys entityKeys = new FlightsSchedulesRequestStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 