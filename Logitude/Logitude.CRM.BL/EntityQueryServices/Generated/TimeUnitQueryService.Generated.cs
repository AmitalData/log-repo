 
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
   public partial class TimeUnitQueryService: EntityQueryService<TimeUnit,TimeUnitKeys,TimeUnitPM,object,TimeUnitKeys>
   {
   
        TimeUnitRepository repository;
		ICRMContext  context;
        public TimeUnitQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new TimeUnitRepository(context);
            Repository = repository;
            mapping = new TimeUnitDataMapping();
        }

        public TimeUnitQueryService(TimeUnitRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TimeUnitDataMapping();
        }

        public TimeUnitQueryService(ICRMContext context)
        {
            this.repository = new TimeUnitRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TimeUnitDataMapping();
        }
		 
		public  TimeUnitPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TimeUnitKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TimeUnit entityPOCO)
        {
            TimeUnitKeys entityKeys = new TimeUnitKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 