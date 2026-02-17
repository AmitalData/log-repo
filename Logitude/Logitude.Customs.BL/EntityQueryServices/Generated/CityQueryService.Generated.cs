 
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
   public partial class CityQueryService: EntityQueryService<City,CityKeys,CityPM,object,CityKeys>
   {
   
        CityRepository repository;
		ICustomContext  context;
        public CityQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CityRepository(context);
            Repository = repository;
            mapping = new CityDataMapping();
        }

        public CityQueryService(CityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CityDataMapping();
        }

        public CityQueryService(ICustomContext context)
        {
            this.repository = new CityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CityDataMapping();
        }
		 
		public  CityPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CityKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(City entityPOCO)
        {
            CityKeys entityKeys = new CityKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 