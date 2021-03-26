 
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
   public partial class ExportDeliveryDocumentMessageSenderCodeQueryService: EntityQueryService<ExportDeliveryDocumentMessageSenderCode,ExportDeliveryDocumentMessageSenderCodeKeys,ExportDeliveryDocumentMessageSenderCodePM,object,ExportDeliveryDocumentMessageSenderCodeKeys>
   {
   
        ExportDeliveryDocumentMessageSenderCodeRepository repository;
		ICustomContext  context;
        public ExportDeliveryDocumentMessageSenderCodeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ExportDeliveryDocumentMessageSenderCodeRepository(context);
            Repository = repository;
            mapping = new ExportDeliveryDocumentMessageSenderCodeDataMapping();
        }

        public ExportDeliveryDocumentMessageSenderCodeQueryService(ExportDeliveryDocumentMessageSenderCodeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExportDeliveryDocumentMessageSenderCodeDataMapping();
        }

        public ExportDeliveryDocumentMessageSenderCodeQueryService(ICustomContext context)
        {
            this.repository = new ExportDeliveryDocumentMessageSenderCodeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExportDeliveryDocumentMessageSenderCodeDataMapping();
        }
		 
		public  ExportDeliveryDocumentMessageSenderCodePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExportDeliveryDocumentMessageSenderCodeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ExportDeliveryDocumentMessageSenderCode entityPOCO)
        {
            ExportDeliveryDocumentMessageSenderCodeKeys entityKeys = new ExportDeliveryDocumentMessageSenderCodeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 