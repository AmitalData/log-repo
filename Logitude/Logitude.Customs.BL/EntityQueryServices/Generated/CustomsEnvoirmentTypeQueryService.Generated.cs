 
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
   public partial class CustomsEnvoirmentTypeQueryService: EntityQueryService<CustomsEnvoirmentType,CustomsEnvoirmentTypeKeys,CustomsEnvoirmentTypePM,object,CustomsEnvoirmentTypeKeys>
   {
   
        CustomsEnvoirmentTypeRepository repository;
		ICustomContext  context;
        public CustomsEnvoirmentTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsEnvoirmentTypeRepository(context);
            Repository = repository;
            mapping = new CustomsEnvoirmentTypeDataMapping();
        }

        public CustomsEnvoirmentTypeQueryService(CustomsEnvoirmentTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsEnvoirmentTypeDataMapping();
        }

        public CustomsEnvoirmentTypeQueryService(ICustomContext context)
        {
            this.repository = new CustomsEnvoirmentTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsEnvoirmentTypeDataMapping();
        }
		 
		public  CustomsEnvoirmentTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsEnvoirmentTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsEnvoirmentType entityPOCO)
        {
            CustomsEnvoirmentTypeKeys entityKeys = new CustomsEnvoirmentTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 