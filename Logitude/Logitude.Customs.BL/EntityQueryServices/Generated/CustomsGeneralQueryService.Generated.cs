 
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
   public partial class CustomsGeneralQueryService: EntityQueryService<CustomsGeneral,CustomsGeneralKeys,CustomsGeneralPM,object,CustomsGeneralKeys>
   {
   
        CustomsGeneralRepository repository;
		ICustomContext  context;
        public CustomsGeneralQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsGeneralRepository(context);
            Repository = repository;
            mapping = new CustomsGeneralDataMapping();
        }

        public CustomsGeneralQueryService(CustomsGeneralRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsGeneralDataMapping();
        }

        public CustomsGeneralQueryService(ICustomContext context)
        {
            this.repository = new CustomsGeneralRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsGeneralDataMapping();
        }
		 
		public  CustomsGeneralPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsGeneralKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsGeneral entityPOCO)
        {
            CustomsGeneralKeys entityKeys = new CustomsGeneralKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 