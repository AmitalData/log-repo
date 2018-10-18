 
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
   public partial class CustomsItemQueryService: EntityQueryService<CustomsItem,CustomsItemKeys,CustomsItemPM,object,CustomsItemKeys>
   {
   
        CustomsItemRepository repository;
		ICustomContext  context;
        public CustomsItemQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsItemRepository(context);
            Repository = repository;
            mapping = new CustomsItemDataMapping();
        }

        public CustomsItemQueryService(CustomsItemRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsItemDataMapping();
        }

        public CustomsItemQueryService(ICustomContext context)
        {
            this.repository = new CustomsItemRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsItemDataMapping();
        }
		 
		public  CustomsItemPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsItemKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsItem entityPOCO)
        {
            CustomsItemKeys entityKeys = new CustomsItemKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 