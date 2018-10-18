 
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
   public partial class RequiredGuaranteeTypeQueryService: EntityQueryService<RequiredGuaranteeType,RequiredGuaranteeTypeKeys,RequiredGuaranteeTypePM,GuaranteePM,GuaranteeKeys>
   {
   
        RequiredGuaranteeTypeRepository repository;
		ICustomContext  context;
        public RequiredGuaranteeTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RequiredGuaranteeTypeRepository(context);
            Repository = repository;
            mapping = new RequiredGuaranteeTypeDataMapping();
        }

        public RequiredGuaranteeTypeQueryService(RequiredGuaranteeTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RequiredGuaranteeTypeDataMapping();
        }

        public RequiredGuaranteeTypeQueryService(ICustomContext context)
        {
            this.repository = new RequiredGuaranteeTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RequiredGuaranteeTypeDataMapping();
        }
		 
		public  RequiredGuaranteeTypePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RequiredGuaranteeTypeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RequiredGuaranteeType entityPOCO)
        {
            RequiredGuaranteeTypeKeys entityKeys = new RequiredGuaranteeTypeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 