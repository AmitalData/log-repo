 
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
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs;
using Amital.QuoteOPM.BL.EntityDataMappings;
using Amital.QuoteOPM.Data.Repsitories;
using Amital.QuoteOPM.Data.EntityKeys;
using Amital.QuoteOPM.Data;

namespace Amital.QuoteOPM.BL.EntityUpdateServices
{ 
   public partial class QuoteOPSettingUpdateService:EntityUpdateService<QuoteOPSetting,QuoteOPSettingPM,EntityPM>
   {
   
        QuoteOPSettingRepository entityRepository;
        public QuoteOPSettingUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IQuoteOPMContext  context = mainContext as QuoteOPMContext;
            context = context ??mainContext as IQuoteOPMContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new QuoteOPSettingDataMapping();
            Repository = new QuoteOPSettingRepository(context);
        }

       
        private IQuoteOPMContext currentContext;
        public QuoteOPSettingUpdateService(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPSettingUpdateService(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(QuoteOPSettingPM entityPM)
        {
            QuoteOPSettingKeys entityKeys = new QuoteOPSettingKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(QuoteOPSettingPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(QuoteOPSettingPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 