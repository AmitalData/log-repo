 
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
   public partial class CRMFilterSettingQueryService: EntityQueryService<CRMFilterSetting,CRMFilterSettingKeys,CRMFilterSettingPM,object,CRMFilterSettingKeys>
   {
   
        CRMFilterSettingRepository repository;
		ICRMContext  context;
        public CRMFilterSettingQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new CRMFilterSettingRepository(context);
            Repository = repository;
            mapping = new CRMFilterSettingDataMapping();
        }

        public CRMFilterSettingQueryService(CRMFilterSettingRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CRMFilterSettingDataMapping();
        }

        public CRMFilterSettingQueryService(ICRMContext context)
        {
            this.repository = new CRMFilterSettingRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CRMFilterSettingDataMapping();
        }
		 
		public  CRMFilterSettingPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CRMFilterSettingKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CRMFilterSetting entityPOCO)
        {
            CRMFilterSettingKeys entityKeys = new CRMFilterSettingKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 