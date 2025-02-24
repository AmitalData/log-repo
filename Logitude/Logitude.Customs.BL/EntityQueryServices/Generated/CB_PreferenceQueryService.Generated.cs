 
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
   public partial class CB_PreferenceQueryService: EntityQueryService<CB_Preference,CB_PreferenceKeys,CB_PreferencePM,object,CB_PreferenceKeys>
   {
   
        CB_PreferenceRepository repository;
		ICustomContext  context;
        public CB_PreferenceQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_PreferenceRepository(context);
            Repository = repository;
            mapping = new CB_PreferenceDataMapping();
        }

        public CB_PreferenceQueryService(CB_PreferenceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_PreferenceDataMapping();
        }

        public CB_PreferenceQueryService(ICustomContext context)
        {
            this.repository = new CB_PreferenceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_PreferenceDataMapping();
        }
		 
		public  CB_PreferencePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_PreferenceKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_Preference entityPOCO)
        {
            CB_PreferenceKeys entityKeys = new CB_PreferenceKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 