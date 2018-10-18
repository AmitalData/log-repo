 
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
   public partial class CustomsCollateralQueryService: EntityQueryService<CustomsCollateral,CustomsCollateralKeys,CustomsCollateralPM,object,CustomsCollateralKeys>
   {
   
        CustomsCollateralRepository repository;
		ICustomContext  context;
        public CustomsCollateralQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsCollateralRepository(context);
            Repository = repository;
            mapping = new CustomsCollateralDataMapping();
        }

        public CustomsCollateralQueryService(CustomsCollateralRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsCollateralDataMapping();
        }

        public CustomsCollateralQueryService(ICustomContext context)
        {
            this.repository = new CustomsCollateralRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsCollateralDataMapping();
        }
		 
		public  CustomsCollateralPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsCollateralKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsCollateral entityPOCO)
        {
            CustomsCollateralKeys entityKeys = new CustomsCollateralKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 