 
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
   public partial class CarWeightQueryService: EntityQueryService<CarWeight,CarWeightKeys,CarWeightPM,object,CarWeightKeys>
   {
   
        CarWeightRepository repository;
		ICustomContext  context;
        public CarWeightQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CarWeightRepository(context);
            Repository = repository;
            mapping = new CarWeightDataMapping();
        }

        public CarWeightQueryService(CarWeightRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CarWeightDataMapping();
        }

        public CarWeightQueryService(ICustomContext context)
        {
            this.repository = new CarWeightRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CarWeightDataMapping();
        }
		 
		public  CarWeightPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CarWeightKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CarWeight entityPOCO)
        {
            CarWeightKeys entityKeys = new CarWeightKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 