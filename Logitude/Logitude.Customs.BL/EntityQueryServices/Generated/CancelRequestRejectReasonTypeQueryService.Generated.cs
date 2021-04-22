 
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
   public partial class CancelRequestRejectReasonTypeQueryService: EntityQueryService<CancelRequestRejectReasonType,CancelRequestRejectReasonTypeKeys,CancelRequestRejectReasonTypePM,object,CancelRequestRejectReasonTypeKeys>
   {
   
        CancelRequestRejectReasonTypeRepository repository;
		ICustomContext  context;
        public CancelRequestRejectReasonTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CancelRequestRejectReasonTypeRepository(context);
            Repository = repository;
            mapping = new CancelRequestRejectReasonTypeDataMapping();
        }

        public CancelRequestRejectReasonTypeQueryService(CancelRequestRejectReasonTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CancelRequestRejectReasonTypeDataMapping();
        }

        public CancelRequestRejectReasonTypeQueryService(ICustomContext context)
        {
            this.repository = new CancelRequestRejectReasonTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CancelRequestRejectReasonTypeDataMapping();
        }
		 
		public  CancelRequestRejectReasonTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CancelRequestRejectReasonTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CancelRequestRejectReasonType entityPOCO)
        {
            CancelRequestRejectReasonTypeKeys entityKeys = new CancelRequestRejectReasonTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 