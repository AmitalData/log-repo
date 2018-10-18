 
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
   public partial class CountryGroupQueryService: EntityQueryService<CountryGroup,CountryGroupKeys,CountryGroupPM,object,CountryGroupKeys>
   {
   
        CountryGroupRepository repository;
		ICustomContext  context;
        public CountryGroupQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CountryGroupRepository(context);
            Repository = repository;
            mapping = new CountryGroupDataMapping();
        }

        public CountryGroupQueryService(CountryGroupRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CountryGroupDataMapping();
        }

        public CountryGroupQueryService(ICustomContext context)
        {
            this.repository = new CountryGroupRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CountryGroupDataMapping();
        }
		 
		public  CountryGroupPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CountryGroupKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CountryGroup entityPOCO)
        {
            CountryGroupKeys entityKeys = new CountryGroupKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 