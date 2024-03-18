 
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
   public partial class CB_ComputationMethodDataQueryService: EntityQueryService<CB_ComputationMethodData,CB_ComputationMethodDataKeys,CB_ComputationMethodDataPM,object,CB_ComputationMethodDataKeys>
   {
   
        CB_ComputationMethodDataRepository repository;
		ICustomContext  context;
        public CB_ComputationMethodDataQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_ComputationMethodDataRepository(context);
            Repository = repository;
            mapping = new CB_ComputationMethodDataDataMapping();
        }

        public CB_ComputationMethodDataQueryService(CB_ComputationMethodDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_ComputationMethodDataDataMapping();
        }

        public CB_ComputationMethodDataQueryService(ICustomContext context)
        {
            this.repository = new CB_ComputationMethodDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_ComputationMethodDataDataMapping();
        }
		 
		public  CB_ComputationMethodDataPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_ComputationMethodDataKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_ComputationMethodData entityPOCO)
        {
            CB_ComputationMethodDataKeys entityKeys = new CB_ComputationMethodDataKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 