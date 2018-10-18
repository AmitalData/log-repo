 
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
   public partial class PriorityQueryService: EntityQueryService<Priority,PriorityKeys,PriorityPM,object,PriorityKeys>
   {
   
        PriorityRepository repository;
		ICRMContext  context;
        public PriorityQueryService(int tenant)
        {
            context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new PriorityRepository(context);
            Repository = repository;
            mapping = new PriorityDataMapping();
        }

        public PriorityQueryService(PriorityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PriorityDataMapping();
        }

        public PriorityQueryService(ICRMContext context)
        {
            this.repository = new PriorityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PriorityDataMapping();
        }
		 
		public  PriorityPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PriorityKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Priority entityPOCO)
        {
            PriorityKeys entityKeys = new PriorityKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 