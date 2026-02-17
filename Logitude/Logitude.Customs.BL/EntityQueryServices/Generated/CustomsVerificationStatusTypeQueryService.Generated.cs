 
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
   public partial class CustomsVerificationStatusTypeQueryService: EntityQueryService<CustomsVerificationStatusType,CustomsVerificationStatusTypeKeys,CustomsVerificationStatusTypePM,object,CustomsVerificationStatusTypeKeys>
   {
   
        CustomsVerificationStatusTypeRepository repository;
		ICustomContext  context;
        public CustomsVerificationStatusTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsVerificationStatusTypeRepository(context);
            Repository = repository;
            mapping = new CustomsVerificationStatusTypeDataMapping();
        }

        public CustomsVerificationStatusTypeQueryService(CustomsVerificationStatusTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsVerificationStatusTypeDataMapping();
        }

        public CustomsVerificationStatusTypeQueryService(ICustomContext context)
        {
            this.repository = new CustomsVerificationStatusTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsVerificationStatusTypeDataMapping();
        }
		 
		public  CustomsVerificationStatusTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsVerificationStatusTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsVerificationStatusType entityPOCO)
        {
            CustomsVerificationStatusTypeKeys entityKeys = new CustomsVerificationStatusTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 