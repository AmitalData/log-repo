 
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
   public partial class ProceduralFaultInSourceTypeQueryService: EntityQueryService<ProceduralFaultInSourceType,ProceduralFaultInSourceTypeKeys,ProceduralFaultInSourceTypePM,object,ProceduralFaultInSourceTypeKeys>
   {
   
        ProceduralFaultInSourceTypeRepository repository;
		ICustomContext  context;
        public ProceduralFaultInSourceTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ProceduralFaultInSourceTypeRepository(context);
            Repository = repository;
            mapping = new ProceduralFaultInSourceTypeDataMapping();
        }

        public ProceduralFaultInSourceTypeQueryService(ProceduralFaultInSourceTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ProceduralFaultInSourceTypeDataMapping();
        }

        public ProceduralFaultInSourceTypeQueryService(ICustomContext context)
        {
            this.repository = new ProceduralFaultInSourceTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ProceduralFaultInSourceTypeDataMapping();
        }
		 
		public  ProceduralFaultInSourceTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ProceduralFaultInSourceTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ProceduralFaultInSourceType entityPOCO)
        {
            ProceduralFaultInSourceTypeKeys entityKeys = new ProceduralFaultInSourceTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 