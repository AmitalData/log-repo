 
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
   public partial class CB_CustomsItemQueryService: EntityQueryService<CB_CustomsItem,CB_CustomsItemKeys,CB_CustomsItemPM,object,CB_CustomsItemKeys>
   {
   
        CB_CustomsItemRepository repository;
		ICustomContext  context;
        public CB_CustomsItemQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_CustomsItemRepository(context);
            Repository = repository;
            mapping = new CB_CustomsItemDataMapping();
        }

        public CB_CustomsItemQueryService(CB_CustomsItemRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_CustomsItemDataMapping();
        }

        public CB_CustomsItemQueryService(ICustomContext context)
        {
            this.repository = new CB_CustomsItemRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_CustomsItemDataMapping();
        }
		 
		public  CB_CustomsItemPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_CustomsItemKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_CustomsItem entityPOCO)
        {
            CB_CustomsItemKeys entityKeys = new CB_CustomsItemKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 