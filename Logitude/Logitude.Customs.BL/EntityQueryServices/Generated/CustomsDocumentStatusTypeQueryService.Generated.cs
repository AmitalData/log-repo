 
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
   public partial class CustomsDocumentStatusTypeQueryService: EntityQueryService<CustomsDocumentStatusType,CustomsDocumentStatusTypeKeys,CustomsDocumentStatusTypePM,object,CustomsDocumentStatusTypeKeys>
   {
   
        CustomsDocumentStatusTypeRepository repository;
		ICustomContext  context;
        public CustomsDocumentStatusTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsDocumentStatusTypeRepository(context);
            Repository = repository;
            mapping = new CustomsDocumentStatusTypeDataMapping();
        }

        public CustomsDocumentStatusTypeQueryService(CustomsDocumentStatusTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsDocumentStatusTypeDataMapping();
        }

        public CustomsDocumentStatusTypeQueryService(ICustomContext context)
        {
            this.repository = new CustomsDocumentStatusTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsDocumentStatusTypeDataMapping();
        }
		 
		public  CustomsDocumentStatusTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsDocumentStatusTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsDocumentStatusType entityPOCO)
        {
            CustomsDocumentStatusTypeKeys entityKeys = new CustomsDocumentStatusTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 