 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityDataMappings;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.CRM.BL.EntityQueryServices
{ 
   public partial class CallTypeQueryService: EntityQueryService<CallType,CallTypeKeys,CallTypePM,object,CallTypeKeys>
   {
   
        CallTypeRepository repository;
		ICRMContext  context;
        public CallTypeQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new CallTypeRepository(context);
            Repository = repository;
            mapping = new CallTypeDataMapping();
        }

        public CallTypeQueryService(CallTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CallTypeDataMapping();
        }

        public CallTypeQueryService(ICRMContext context)
        {
            this.repository = new CallTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CallTypeDataMapping();
        }
		 
		public  CallTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CallTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CallType entityPOCO)
        {
            CallTypeKeys entityKeys = new CallTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 