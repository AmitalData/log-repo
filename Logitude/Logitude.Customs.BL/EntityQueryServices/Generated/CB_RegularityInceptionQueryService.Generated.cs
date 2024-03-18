 
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
   public partial class CB_RegularityInceptionQueryService: EntityQueryService<CB_RegularityInception,CB_RegularityInceptionKeys,CB_RegularityInceptionPM,object,CB_RegularityInceptionKeys>
   {
   
        CB_RegularityInceptionRepository repository;
		ICustomContext  context;
        public CB_RegularityInceptionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_RegularityInceptionRepository(context);
            Repository = repository;
            mapping = new CB_RegularityInceptionDataMapping();
        }

        public CB_RegularityInceptionQueryService(CB_RegularityInceptionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_RegularityInceptionDataMapping();
        }

        public CB_RegularityInceptionQueryService(ICustomContext context)
        {
            this.repository = new CB_RegularityInceptionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_RegularityInceptionDataMapping();
        }
		 
		public  CB_RegularityInceptionPM GetSingle(int id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_RegularityInceptionKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_RegularityInception entityPOCO)
        {
            CB_RegularityInceptionKeys entityKeys = new CB_RegularityInceptionKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 