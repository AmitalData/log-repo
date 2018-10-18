 
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
   public partial class CustomsRequestsSheetQueryService: EntityQueryService<CustomsRequestsSheet,CustomsRequestsSheetKeys,CustomsRequestsSheetPM,object,CustomsRequestsSheetKeys>
   {
   
        CustomsRequestsSheetRepository repository;
		ICustomContext  context;
        public CustomsRequestsSheetQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsRequestsSheetRepository(context);
            Repository = repository;
            mapping = new CustomsRequestsSheetDataMapping();
        }

        public CustomsRequestsSheetQueryService(CustomsRequestsSheetRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsRequestsSheetDataMapping();
        }

        public CustomsRequestsSheetQueryService(ICustomContext context)
        {
            this.repository = new CustomsRequestsSheetRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsRequestsSheetDataMapping();
        }
		 
		public  CustomsRequestsSheetPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsRequestsSheetKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsRequestsSheet entityPOCO)
        {
            CustomsRequestsSheetKeys entityKeys = new CustomsRequestsSheetKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 