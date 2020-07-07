 
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
   public partial class CancellationReasonRequestTypeQueryService: EntityQueryService<CancellationReasonRequestType,CancellationReasonRequestTypeKeys,CancellationReasonRequestTypePM,object,CancellationReasonRequestTypeKeys>
   {
   
        CancellationReasonRequestTypeRepository repository;
		ICustomContext  context;
        public CancellationReasonRequestTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CancellationReasonRequestTypeRepository(context);
            Repository = repository;
            mapping = new CancellationReasonRequestTypeDataMapping();
        }

        public CancellationReasonRequestTypeQueryService(CancellationReasonRequestTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CancellationReasonRequestTypeDataMapping();
        }

        public CancellationReasonRequestTypeQueryService(ICustomContext context)
        {
            this.repository = new CancellationReasonRequestTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CancellationReasonRequestTypeDataMapping();
        }
		 
		public  CancellationReasonRequestTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CancellationReasonRequestTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CancellationReasonRequestType entityPOCO)
        {
            CancellationReasonRequestTypeKeys entityKeys = new CancellationReasonRequestTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 