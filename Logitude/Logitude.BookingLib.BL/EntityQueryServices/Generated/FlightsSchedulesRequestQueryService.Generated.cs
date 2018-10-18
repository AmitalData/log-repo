 
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
   public partial class FlightsSchedulesRequestQueryService: EntityQueryService<FlightsSchedulesRequest,FlightsSchedulesRequestKeys,FlightsSchedulesRequestPM,object,FlightsSchedulesRequestKeys>
   {
   
        FlightsSchedulesRequestRepository repository;
		IBookingContext  context;
        public FlightsSchedulesRequestQueryService(int tenant)
        {
		    context = BookingContext.GetContext(tenant);
            MainContext = context;
            repository = new FlightsSchedulesRequestRepository(context);
            Repository = repository;
            mapping = new FlightsSchedulesRequestDataMapping();
        }

        public FlightsSchedulesRequestQueryService(FlightsSchedulesRequestRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new FlightsSchedulesRequestDataMapping();
        }

        public FlightsSchedulesRequestQueryService(IBookingContext context)
        {
            this.repository = new FlightsSchedulesRequestRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new FlightsSchedulesRequestDataMapping();
        }
		 
		public  FlightsSchedulesRequestPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new FlightsSchedulesRequestKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(FlightsSchedulesRequest entityPOCO)
        {
            FlightsSchedulesRequestKeys entityKeys = new FlightsSchedulesRequestKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 