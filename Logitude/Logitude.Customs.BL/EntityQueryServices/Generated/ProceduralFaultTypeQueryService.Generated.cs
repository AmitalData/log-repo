 
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
   public partial class ProceduralFaultTypeQueryService: EntityQueryService<ProceduralFaultType,ProceduralFaultTypeKeys,ProceduralFaultTypePM,object,ProceduralFaultTypeKeys>
   {
   
        ProceduralFaultTypeRepository repository;
		ICustomContext  context;
        public ProceduralFaultTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ProceduralFaultTypeRepository(context);
            Repository = repository;
            mapping = new ProceduralFaultTypeDataMapping();
        }

        public ProceduralFaultTypeQueryService(ProceduralFaultTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ProceduralFaultTypeDataMapping();
        }

        public ProceduralFaultTypeQueryService(ICustomContext context)
        {
            this.repository = new ProceduralFaultTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ProceduralFaultTypeDataMapping();
        }
		 
		public  ProceduralFaultTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ProceduralFaultTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ProceduralFaultType entityPOCO)
        {
            ProceduralFaultTypeKeys entityKeys = new ProceduralFaultTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 