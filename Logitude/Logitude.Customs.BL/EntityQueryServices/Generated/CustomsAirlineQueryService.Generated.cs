 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class CustomsAirlineQueryService: EntityQueryService<CustomsAirline,CustomsAirlineKeys,CustomsAirlinePM,object,CustomsAirlineKeys>
   {
   
        CustomsAirlineRepository repository;
		ICustomContext  context;
        public CustomsAirlineQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsAirlineRepository(context);
            Repository = repository;
            mapping = new CustomsAirlineDataMapping();
        }

        public CustomsAirlineQueryService(CustomsAirlineRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsAirlineDataMapping();
        }

        public CustomsAirlineQueryService(ICustomContext context)
        {
            this.repository = new CustomsAirlineRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsAirlineDataMapping();
        }
		 
		public  CustomsAirlinePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsAirlineKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsAirline entityPOCO)
        {
            CustomsAirlineKeys entityKeys = new CustomsAirlineKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 