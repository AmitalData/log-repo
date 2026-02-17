 
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
   public partial class MeasurmentUnitQueryService: EntityQueryService<MeasurmentUnit,MeasurmentUnitKeys,MeasurmentUnitPM,object,MeasurmentUnitKeys>
   {
   
        MeasurmentUnitRepository repository;
		ICustomContext  context;
        public MeasurmentUnitQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new MeasurmentUnitRepository(context);
            Repository = repository;
            mapping = new MeasurmentUnitDataMapping();
        }

        public MeasurmentUnitQueryService(MeasurmentUnitRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new MeasurmentUnitDataMapping();
        }

        public MeasurmentUnitQueryService(ICustomContext context)
        {
            this.repository = new MeasurmentUnitRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new MeasurmentUnitDataMapping();
        }
		 
		public  MeasurmentUnitPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new MeasurmentUnitKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(MeasurmentUnit entityPOCO)
        {
            MeasurmentUnitKeys entityKeys = new MeasurmentUnitKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 