 
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
   public partial class GovernmentProcedureTypeQueryService: EntityQueryService<GovernmentProcedureType,GovernmentProcedureTypeKeys,GovernmentProcedureTypePM,object,GovernmentProcedureTypeKeys>
   {
   
        GovernmentProcedureTypeRepository repository;
		ICustomContext  context;
        public GovernmentProcedureTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new GovernmentProcedureTypeRepository(context);
            Repository = repository;
            mapping = new GovernmentProcedureTypeDataMapping();
        }

        public GovernmentProcedureTypeQueryService(GovernmentProcedureTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GovernmentProcedureTypeDataMapping();
        }

        public GovernmentProcedureTypeQueryService(ICustomContext context)
        {
            this.repository = new GovernmentProcedureTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GovernmentProcedureTypeDataMapping();
        }
		 
		public  GovernmentProcedureTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GovernmentProcedureTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GovernmentProcedureType entityPOCO)
        {
            GovernmentProcedureTypeKeys entityKeys = new GovernmentProcedureTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 