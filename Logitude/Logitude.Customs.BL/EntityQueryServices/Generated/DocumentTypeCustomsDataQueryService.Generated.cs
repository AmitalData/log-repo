 
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
   public partial class DocumentTypeCustomsDataQueryService: EntityQueryService<DocumentTypeCustomsData,DocumentTypeCustomsDataKeys,DocumentTypeCustomsDataPM,object,DocumentTypeCustomsDataKeys>
   {
   
        DocumentTypeCustomsDataRepository repository;
		ICustomContext  context;
        public DocumentTypeCustomsDataQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DocumentTypeCustomsDataRepository(context);
            Repository = repository;
            mapping = new DocumentTypeCustomsDataDataMapping();
        }

        public DocumentTypeCustomsDataQueryService(DocumentTypeCustomsDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DocumentTypeCustomsDataDataMapping();
        }

        public DocumentTypeCustomsDataQueryService(ICustomContext context)
        {
            this.repository = new DocumentTypeCustomsDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DocumentTypeCustomsDataDataMapping();
        }
		 
		public  DocumentTypeCustomsDataPM GetSingle(string documenttypeid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DocumentTypeCustomsDataKeys(){ DocumentTypeId = documenttypeid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DocumentTypeCustomsData entityPOCO)
        {
            DocumentTypeCustomsDataKeys entityKeys = new DocumentTypeCustomsDataKeys() { DocumentTypeId = entityPOCO.DocumentTypeId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 