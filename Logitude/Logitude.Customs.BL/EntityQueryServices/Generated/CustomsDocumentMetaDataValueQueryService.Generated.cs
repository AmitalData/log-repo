 
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
   public partial class CustomsDocumentMetaDataValueQueryService: EntityQueryService<CustomsDocumentMetaDataValue,CustomsDocumentMetaDataValueKeys,CustomsDocumentMetaDataValuePM,CustomsDocumentPM,CustomsDocumentKeys>
   {
   
        CustomsDocumentMetaDataValueRepository repository;
		ICustomContext  context;
        public CustomsDocumentMetaDataValueQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsDocumentMetaDataValueRepository(context);
            Repository = repository;
            mapping = new CustomsDocumentMetaDataValueDataMapping();
        }

        public CustomsDocumentMetaDataValueQueryService(CustomsDocumentMetaDataValueRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsDocumentMetaDataValueDataMapping();
        }

        public CustomsDocumentMetaDataValueQueryService(ICustomContext context)
        {
            this.repository = new CustomsDocumentMetaDataValueRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsDocumentMetaDataValueDataMapping();
        }
		 
		public  CustomsDocumentMetaDataValuePM GetSingle(string customsdocumentid, string metadatatypecode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsDocumentMetaDataValueKeys(){ CustomsDocumentId = customsdocumentid, MetaDataTypeCode = metadatatypecode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsDocumentMetaDataValue entityPOCO)
        {
            CustomsDocumentMetaDataValueKeys entityKeys = new CustomsDocumentMetaDataValueKeys() { CustomsDocumentId = entityPOCO.CustomsDocumentId, MetaDataTypeCode = entityPOCO.MetaDataTypeCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 