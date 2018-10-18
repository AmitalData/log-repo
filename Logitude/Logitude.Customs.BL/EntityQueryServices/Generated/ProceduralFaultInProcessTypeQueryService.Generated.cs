 
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
   public partial class ProceduralFaultInProcessTypeQueryService: EntityQueryService<ProceduralFaultInProcessType,ProceduralFaultInProcessTypeKeys,ProceduralFaultInProcessTypePM,object,ProceduralFaultInProcessTypeKeys>
   {
   
        ProceduralFaultInProcessTypeRepository repository;
		ICustomContext  context;
        public ProceduralFaultInProcessTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ProceduralFaultInProcessTypeRepository(context);
            Repository = repository;
            mapping = new ProceduralFaultInProcessTypeDataMapping();
        }

        public ProceduralFaultInProcessTypeQueryService(ProceduralFaultInProcessTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ProceduralFaultInProcessTypeDataMapping();
        }

        public ProceduralFaultInProcessTypeQueryService(ICustomContext context)
        {
            this.repository = new ProceduralFaultInProcessTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ProceduralFaultInProcessTypeDataMapping();
        }
		 
		public  ProceduralFaultInProcessTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ProceduralFaultInProcessTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ProceduralFaultInProcessType entityPOCO)
        {
            ProceduralFaultInProcessTypeKeys entityKeys = new ProceduralFaultInProcessTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 