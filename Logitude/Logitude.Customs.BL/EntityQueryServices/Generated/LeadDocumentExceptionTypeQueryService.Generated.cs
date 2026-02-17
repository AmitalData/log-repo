 
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
   public partial class LeadDocumentExceptionTypeQueryService: EntityQueryService<LeadDocumentExceptionType,LeadDocumentExceptionTypeKeys,LeadDocumentExceptionTypePM,object,LeadDocumentExceptionTypeKeys>
   {
   
        LeadDocumentExceptionTypeRepository repository;
		ICustomContext  context;
        public LeadDocumentExceptionTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new LeadDocumentExceptionTypeRepository(context);
            Repository = repository;
            mapping = new LeadDocumentExceptionTypeDataMapping();
        }

        public LeadDocumentExceptionTypeQueryService(LeadDocumentExceptionTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new LeadDocumentExceptionTypeDataMapping();
        }

        public LeadDocumentExceptionTypeQueryService(ICustomContext context)
        {
            this.repository = new LeadDocumentExceptionTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new LeadDocumentExceptionTypeDataMapping();
        }
		 
		public  LeadDocumentExceptionTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new LeadDocumentExceptionTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(LeadDocumentExceptionType entityPOCO)
        {
            LeadDocumentExceptionTypeKeys entityKeys = new LeadDocumentExceptionTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 