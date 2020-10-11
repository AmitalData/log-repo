 
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
   public partial class AutonomyRegionTypeQueryService: EntityQueryService<AutonomyRegionType,AutonomyRegionTypeKeys,AutonomyRegionTypePM,object,AutonomyRegionTypeKeys>
   {
   
        AutonomyRegionTypeRepository repository;
		ICustomContext  context;
        public AutonomyRegionTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AutonomyRegionTypeRepository(context);
            Repository = repository;
            mapping = new AutonomyRegionTypeDataMapping();
        }

        public AutonomyRegionTypeQueryService(AutonomyRegionTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AutonomyRegionTypeDataMapping();
        }

        public AutonomyRegionTypeQueryService(ICustomContext context)
        {
            this.repository = new AutonomyRegionTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AutonomyRegionTypeDataMapping();
        }
		 
		public  AutonomyRegionTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AutonomyRegionTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AutonomyRegionType entityPOCO)
        {
            AutonomyRegionTypeKeys entityKeys = new AutonomyRegionTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 