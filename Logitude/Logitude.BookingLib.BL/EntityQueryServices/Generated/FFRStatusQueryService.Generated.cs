 
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
   public partial class FFRStatusQueryService: EntityQueryService<FFRStatus,FFRStatusKeys,FFRStatusPM,object,FFRStatusKeys>
   {
   
        FFRStatusRepository repository;
		IBookingContext  context;
        public FFRStatusQueryService(int tenant)
        {
		    context = BookingContext.GetContext(tenant);
            MainContext = context;
            repository = new FFRStatusRepository(context);
            Repository = repository;
            mapping = new FFRStatusDataMapping();
        }

        public FFRStatusQueryService(FFRStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new FFRStatusDataMapping();
        }

        public FFRStatusQueryService(IBookingContext context)
        {
            this.repository = new FFRStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new FFRStatusDataMapping();
        }
		 
		public  FFRStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new FFRStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(FFRStatus entityPOCO)
        {
            FFRStatusKeys entityKeys = new FFRStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 