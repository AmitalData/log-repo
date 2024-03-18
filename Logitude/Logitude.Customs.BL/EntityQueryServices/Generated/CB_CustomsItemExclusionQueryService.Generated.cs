 
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
   public partial class CB_CustomsItemExclusionQueryService: EntityQueryService<CB_CustomsItemExclusion,CB_CustomsItemExclusionKeys,CB_CustomsItemExclusionPM,object,CB_CustomsItemExclusionKeys>
   {
   
        CB_CustomsItemExclusionRepository repository;
		ICustomContext  context;
        public CB_CustomsItemExclusionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_CustomsItemExclusionRepository(context);
            Repository = repository;
            mapping = new CB_CustomsItemExclusionDataMapping();
        }

        public CB_CustomsItemExclusionQueryService(CB_CustomsItemExclusionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_CustomsItemExclusionDataMapping();
        }

        public CB_CustomsItemExclusionQueryService(ICustomContext context)
        {
            this.repository = new CB_CustomsItemExclusionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_CustomsItemExclusionDataMapping();
        }
		 
		public  CB_CustomsItemExclusionPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_CustomsItemExclusionKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_CustomsItemExclusion entityPOCO)
        {
            CB_CustomsItemExclusionKeys entityKeys = new CB_CustomsItemExclusionKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 