 
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
   public partial class CustomsRequestsSheetStatusQueryService: EntityQueryService<CustomsRequestsSheetStatus,CustomsRequestsSheetStatusKeys,CustomsRequestsSheetStatusPM,object,CustomsRequestsSheetStatusKeys>
   {
   
        CustomsRequestsSheetStatusRepository repository;
		ICustomContext  context;
        public CustomsRequestsSheetStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CustomsRequestsSheetStatusRepository(context);
            Repository = repository;
            mapping = new CustomsRequestsSheetStatusDataMapping();
        }

        public CustomsRequestsSheetStatusQueryService(CustomsRequestsSheetStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CustomsRequestsSheetStatusDataMapping();
        }

        public CustomsRequestsSheetStatusQueryService(ICustomContext context)
        {
            this.repository = new CustomsRequestsSheetStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CustomsRequestsSheetStatusDataMapping();
        }
		 
		public  CustomsRequestsSheetStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CustomsRequestsSheetStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CustomsRequestsSheetStatus entityPOCO)
        {
            CustomsRequestsSheetStatusKeys entityKeys = new CustomsRequestsSheetStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 