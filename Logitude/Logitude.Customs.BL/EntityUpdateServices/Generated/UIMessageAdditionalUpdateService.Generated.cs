 
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
   public partial class UIMessageAdditionalUpdateService:EntityUpdateService<UIMessageAdditional,UIMessageAdditionalPM,EntityPM>
   {
   
        UIMessageAdditionalRepository entityRepository;
        public UIMessageAdditionalUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            context = context ??mainContext as ICustomContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new UIMessageAdditionalDataMapping();
            Repository = new UIMessageAdditionalRepository(context);
        }

       
        private ICustomContext currentContext;
        public UIMessageAdditionalUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public UIMessageAdditionalUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(UIMessageAdditionalPM entityPM)
        {
            UIMessageAdditionalKeys entityKeys = new UIMessageAdditionalKeys() { Id = entityPM.Id, Code = entityPM.Code };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(UIMessageAdditionalPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(UIMessageAdditionalPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 