 
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
   public partial class CustomDocumentTypeQueryService: EntityQueryService<CustomDocumentType,CustomDocumentTypeKeys,CustomDocumentTypePM,object,CustomDocumentTypeKeys>
   {
   
        CustomDocumentTypeRepository repository;
		ICustomContext  context;
        public CustomDocumentTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomDocumentTypeRepository(context);
            Repository = repository;
            mapping = new CustomDocumentTypeDataMapping();
        }

        public CustomDocumentTypeQueryService(CustomDocumentTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomDocumentTypeDataMapping();
        }

        public CustomDocumentTypeQueryService(ICustomContext context)
        {
            this.repository = new CustomDocumentTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomDocumentTypeDataMapping();
        }
		 
		public  CustomDocumentTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomDocumentTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomDocumentType entityPOCO)
        {
            CustomDocumentTypeKeys entityKeys = new CustomDocumentTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 