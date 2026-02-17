 
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
   public partial class CustomsDocumentPointerQueryService: EntityQueryService<CustomsDocumentPointer,CustomsDocumentPointerKeys,CustomsDocumentPointerPM,CustomsDocumentsTicketPM,CustomsDocumentsTicketKeys>
   {
   
        CustomsDocumentPointerRepository repository;
		ICustomContext  context;
        public CustomsDocumentPointerQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsDocumentPointerRepository(context);
            Repository = repository;
            mapping = new CustomsDocumentPointerDataMapping();
        }

        public CustomsDocumentPointerQueryService(CustomsDocumentPointerRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsDocumentPointerDataMapping();
        }

        public CustomsDocumentPointerQueryService(ICustomContext context)
        {
            this.repository = new CustomsDocumentPointerRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsDocumentPointerDataMapping();
        }
		 
		public  CustomsDocumentPointerPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsDocumentPointerKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsDocumentPointer entityPOCO)
        {
            CustomsDocumentPointerKeys entityKeys = new CustomsDocumentPointerKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 