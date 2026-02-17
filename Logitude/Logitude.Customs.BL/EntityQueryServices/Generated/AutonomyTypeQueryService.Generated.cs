 
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
   public partial class AutonomyTypeQueryService: EntityQueryService<AutonomyType,AutonomyTypeKeys,AutonomyTypePM,object,AutonomyTypeKeys>
   {
   
        AutonomyTypeRepository repository;
		ICustomContext  context;
        public AutonomyTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AutonomyTypeRepository(context);
            Repository = repository;
            mapping = new AutonomyTypeDataMapping();
        }

        public AutonomyTypeQueryService(AutonomyTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AutonomyTypeDataMapping();
        }

        public AutonomyTypeQueryService(ICustomContext context)
        {
            this.repository = new AutonomyTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AutonomyTypeDataMapping();
        }
		 
		public  AutonomyTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AutonomyTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AutonomyType entityPOCO)
        {
            AutonomyTypeKeys entityKeys = new AutonomyTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 