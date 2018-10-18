 
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
   public partial class LeadDocumentTypeQueryService: EntityQueryService<LeadDocumentType,LeadDocumentTypeKeys,LeadDocumentTypePM,object,LeadDocumentTypeKeys>
   {
   
        LeadDocumentTypeRepository repository;
		ICustomContext  context;
        public LeadDocumentTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new LeadDocumentTypeRepository(context);
            Repository = repository;
            mapping = new LeadDocumentTypeDataMapping();
        }

        public LeadDocumentTypeQueryService(LeadDocumentTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new LeadDocumentTypeDataMapping();
        }

        public LeadDocumentTypeQueryService(ICustomContext context)
        {
            this.repository = new LeadDocumentTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new LeadDocumentTypeDataMapping();
        }
		 
		public  LeadDocumentTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new LeadDocumentTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(LeadDocumentType entityPOCO)
        {
            LeadDocumentTypeKeys entityKeys = new LeadDocumentTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 