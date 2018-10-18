 
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
   public partial class CustomDocumentTypeMetaDataQueryService: EntityQueryService<CustomDocumentTypeMetaData,CustomDocumentTypeMetaDataKeys,CustomDocumentTypeMetaDataPM,object,CustomDocumentTypeMetaDataKeys>
   {
   
        CustomDocumentTypeMetaDataRepository repository;
		ICustomContext  context;
        public CustomDocumentTypeMetaDataQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomDocumentTypeMetaDataRepository(context);
            Repository = repository;
            mapping = new CustomDocumentTypeMetaDataDataMapping();
        }

        public CustomDocumentTypeMetaDataQueryService(CustomDocumentTypeMetaDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomDocumentTypeMetaDataDataMapping();
        }

        public CustomDocumentTypeMetaDataQueryService(ICustomContext context)
        {
            this.repository = new CustomDocumentTypeMetaDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomDocumentTypeMetaDataDataMapping();
        }
		 
		public  CustomDocumentTypeMetaDataPM GetSingle(string metadatatypecode, string documenttypecode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomDocumentTypeMetaDataKeys(){ MetaDataTypeCode = metadatatypecode, DocumentTypeCode = documenttypecode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomDocumentTypeMetaData entityPOCO)
        {
            CustomDocumentTypeMetaDataKeys entityKeys = new CustomDocumentTypeMetaDataKeys() { MetaDataTypeCode = entityPOCO.MetaDataTypeCode, DocumentTypeCode = entityPOCO.DocumentTypeCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 