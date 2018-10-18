 
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
   public partial class CustomsCountryQueryService: EntityQueryService<CustomsCountry,CustomsCountryKeys,CustomsCountryPM,object,CustomsCountryKeys>
   {
   
        CustomsCountryRepository repository;
		ICustomContext  context;
        public CustomsCountryQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsCountryRepository(context);
            Repository = repository;
            mapping = new CustomsCountryDataMapping();
        }

        public CustomsCountryQueryService(CustomsCountryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsCountryDataMapping();
        }

        public CustomsCountryQueryService(ICustomContext context)
        {
            this.repository = new CustomsCountryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsCountryDataMapping();
        }
		 
		public  CustomsCountryPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsCountryKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsCountry entityPOCO)
        {
            CustomsCountryKeys entityKeys = new CustomsCountryKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 