 
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
   public partial class FlightsSchedulesResponseQueryService: EntityQueryService<FlightsSchedulesResponse,FlightsSchedulesResponseKeys,FlightsSchedulesResponsePM,FlightsSchedulesRequestPM,FlightsSchedulesRequestKeys>
   {
   
        FlightsSchedulesResponseRepository repository;
		IBookingContext  context;
        public FlightsSchedulesResponseQueryService(int tenant)
        {
		    context = BookingContext.GetContext(tenant);
            MainContext = context;
            repository = new FlightsSchedulesResponseRepository(context);
            Repository = repository;
            mapping = new FlightsSchedulesResponseDataMapping();
        }

        public FlightsSchedulesResponseQueryService(FlightsSchedulesResponseRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new FlightsSchedulesResponseDataMapping();
        }

        public FlightsSchedulesResponseQueryService(IBookingContext context)
        {
            this.repository = new FlightsSchedulesResponseRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new FlightsSchedulesResponseDataMapping();
        }
		 
		public  FlightsSchedulesResponsePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new FlightsSchedulesResponseKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(FlightsSchedulesResponse entityPOCO)
        {
            FlightsSchedulesResponseKeys entityKeys = new FlightsSchedulesResponseKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 