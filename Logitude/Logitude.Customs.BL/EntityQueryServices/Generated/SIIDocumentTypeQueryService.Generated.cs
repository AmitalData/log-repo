 
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
   public partial class SIIDocumentTypeQueryService: EntityQueryService<SIIDocumentType,SIIDocumentTypeKeys,SIIDocumentTypePM,object,SIIDocumentTypeKeys>
   {
   
        SIIDocumentTypeRepository repository;
		ICustomContext  context;
        public SIIDocumentTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new SIIDocumentTypeRepository(context);
            Repository = repository;
            mapping = new SIIDocumentTypeDataMapping();
        }

        public SIIDocumentTypeQueryService(SIIDocumentTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SIIDocumentTypeDataMapping();
        }

        public SIIDocumentTypeQueryService(ICustomContext context)
        {
            this.repository = new SIIDocumentTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SIIDocumentTypeDataMapping();
        }
		 
		public  SIIDocumentTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SIIDocumentTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SIIDocumentType entityPOCO)
        {
            SIIDocumentTypeKeys entityKeys = new SIIDocumentTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 