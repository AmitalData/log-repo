 
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
   public partial class CustomMetaDataTypeQueryService: EntityQueryService<CustomMetaDataType,CustomMetaDataTypeKeys,CustomMetaDataTypePM,object,CustomMetaDataTypeKeys>
   {
   
        CustomMetaDataTypeRepository repository;
		ICustomContext  context;
        public CustomMetaDataTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomMetaDataTypeRepository(context);
            Repository = repository;
            mapping = new CustomMetaDataTypeDataMapping();
        }

        public CustomMetaDataTypeQueryService(CustomMetaDataTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomMetaDataTypeDataMapping();
        }

        public CustomMetaDataTypeQueryService(ICustomContext context)
        {
            this.repository = new CustomMetaDataTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomMetaDataTypeDataMapping();
        }
		 
		public  CustomMetaDataTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomMetaDataTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomMetaDataType entityPOCO)
        {
            CustomMetaDataTypeKeys entityKeys = new CustomMetaDataTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 