 
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
   public partial class LeadDocumentTypeUpdateService:EntityUpdateService<LeadDocumentType,LeadDocumentTypePM,EntityPM>
   {
   
        LeadDocumentTypeRepository entityRepository;
        public LeadDocumentTypeUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            context = context ??mainContext as ICustomContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new LeadDocumentTypeDataMapping();
            Repository = new LeadDocumentTypeRepository(context);
        }

       
        private ICustomContext currentContext;
        public LeadDocumentTypeUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public LeadDocumentTypeUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(LeadDocumentTypePM entityPM)
        {
            LeadDocumentTypeKeys entityKeys = new LeadDocumentTypeKeys() { Code = entityPM.Code };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(LeadDocumentTypePM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(LeadDocumentTypePM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 