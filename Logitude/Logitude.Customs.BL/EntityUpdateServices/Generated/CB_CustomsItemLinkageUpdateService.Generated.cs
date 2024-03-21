 
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
   public partial class CB_CustomsItemLinkageUpdateService:EntityUpdateService<CB_CustomsItemLinkage,CB_CustomsItemLinkagePM,EntityPM>
   {
   
        CB_CustomsItemLinkageRepository entityRepository;
        public CB_CustomsItemLinkageUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            context = context ??mainContext as ICustomContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new CB_CustomsItemLinkageDataMapping();
            Repository = new CB_CustomsItemLinkageRepository(context);
        }

       
        private ICustomContext currentContext;
        public CB_CustomsItemLinkageUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_CustomsItemLinkageUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(CB_CustomsItemLinkagePM entityPM)
        {
            CB_CustomsItemLinkageKeys entityKeys = new CB_CustomsItemLinkageKeys() { CB_ID = entityPM.CB_ID };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(CB_CustomsItemLinkagePM entityPM)
        {     
  
		
	    }
        
		protected override void FillDefaultValuesOnUpdate(CB_CustomsItemLinkagePM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 