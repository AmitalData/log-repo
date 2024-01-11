 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityUpdateServices
{ 
   public partial class CB_PropertiesDetailsHistoryUpdateService:EntityUpdateService<CB_PropertiesDetailsHistory,CB_PropertiesDetailsHistoryPM,CB_CustomsItemPM>
   {
   
        CB_PropertiesDetailsHistoryRepository entityRepository;
        public CB_PropertiesDetailsHistoryUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            context = context ??mainContext as ICustomContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new CB_PropertiesDetailsHistoryDataMapping();
            Repository = new CB_PropertiesDetailsHistoryRepository(context);
        }

       
        private ICustomContext currentContext;
        public CB_PropertiesDetailsHistoryUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_PropertiesDetailsHistoryUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(CB_PropertiesDetailsHistoryPM entityPM)
        {
            CB_PropertiesDetailsHistoryKeys entityKeys = new CB_PropertiesDetailsHistoryKeys() { ID = entityPM.ID };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(CB_PropertiesDetailsHistoryPM entityPM)
        {     
  
		
	    }
        
		protected override void FillDefaultValuesOnUpdate(CB_PropertiesDetailsHistoryPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 