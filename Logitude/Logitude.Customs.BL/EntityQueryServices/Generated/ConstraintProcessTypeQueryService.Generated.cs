 
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
   public partial class ConstraintProcessTypeQueryService: EntityQueryService<ConstraintProcessType,ConstraintProcessTypeKeys,ConstraintProcessTypePM,object,ConstraintProcessTypeKeys>
   {
   
        ConstraintProcessTypeRepository repository;
		ICustomContext  context;
        public ConstraintProcessTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ConstraintProcessTypeRepository(context);
            Repository = repository;
            mapping = new ConstraintProcessTypeDataMapping();
        }

        public ConstraintProcessTypeQueryService(ConstraintProcessTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ConstraintProcessTypeDataMapping();
        }

        public ConstraintProcessTypeQueryService(ICustomContext context)
        {
            this.repository = new ConstraintProcessTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ConstraintProcessTypeDataMapping();
        }
		 
		public  ConstraintProcessTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ConstraintProcessTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ConstraintProcessType entityPOCO)
        {
            ConstraintProcessTypeKeys entityKeys = new ConstraintProcessTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 