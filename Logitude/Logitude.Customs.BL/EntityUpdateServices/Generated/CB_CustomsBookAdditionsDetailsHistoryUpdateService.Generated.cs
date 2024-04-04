 
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
   public partial class CB_CustomsBookAdditionsDetailsHistoryUpdateService:EntityUpdateService<CB_CustomsBookAdditionsDetailsHistory,CB_CustomsBookAdditionsDetailsHistoryPM,EntityPM>
   {
   
        CB_CustomsBookAdditionsDetailsHistoryRepository entityRepository;
        public CB_CustomsBookAdditionsDetailsHistoryUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            context = context ??mainContext as ICustomContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new CB_CustomsBookAdditionsDetailsHistoryDataMapping();
            Repository = new CB_CustomsBookAdditionsDetailsHistoryRepository(context);
        }

       
        private ICustomContext currentContext;
        public CB_CustomsBookAdditionsDetailsHistoryUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_CustomsBookAdditionsDetailsHistoryUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(CB_CustomsBookAdditionsDetailsHistoryPM entityPM)
        {
            CB_CustomsBookAdditionsDetailsHistoryKeys entityKeys = new CB_CustomsBookAdditionsDetailsHistoryKeys() { CB_ID = entityPM.CB_ID };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(CB_CustomsBookAdditionsDetailsHistoryPM entityPM)
        {     
  
		
	    }
        
		protected override void FillDefaultValuesOnUpdate(CB_CustomsBookAdditionsDetailsHistoryPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 