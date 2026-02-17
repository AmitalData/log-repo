 
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
   public partial class CustomsBookTypeQueryService: EntityQueryService<CustomsBookType,CustomsBookTypeKeys,CustomsBookTypePM,object,CustomsBookTypeKeys>
   {
   
        CustomsBookTypeRepository repository;
		ICustomContext  context;
        public CustomsBookTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsBookTypeRepository(context);
            Repository = repository;
            mapping = new CustomsBookTypeDataMapping();
        }

        public CustomsBookTypeQueryService(CustomsBookTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsBookTypeDataMapping();
        }

        public CustomsBookTypeQueryService(ICustomContext context)
        {
            this.repository = new CustomsBookTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsBookTypeDataMapping();
        }
		 
		public  CustomsBookTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsBookTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsBookType entityPOCO)
        {
            CustomsBookTypeKeys entityKeys = new CustomsBookTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 