 
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
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.BL.EntityDataMappings;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityKeys;
using Logitude.TariffModule.Data;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{ 
   public partial class TariffVersionUploadedExcelUpdateService:EntityUpdateService<TariffVersionUploadedExcel,TariffVersionUploadedExcelPM,EntityPM>
   {
   
        TariffVersionUploadedExcelRepository entityRepository;
        public TariffVersionUploadedExcelUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ITariffModuleContext  context = mainContext as TariffModuleContext;
            context = context ??mainContext as ITariffModuleContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new TariffVersionUploadedExcelDataMapping();
            Repository = new TariffVersionUploadedExcelRepository(context);
        }

       
        private ITariffModuleContext currentContext;
        public TariffVersionUploadedExcelUpdateService(int tenant)
        {
            currentContext = TariffModuleContext.GetContext(tenant);
        }

        public TariffVersionUploadedExcelUpdateService(ITariffModuleContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(TariffVersionUploadedExcelPM entityPM)
        {
            TariffVersionUploadedExcelKeys entityKeys = new TariffVersionUploadedExcelKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(TariffVersionUploadedExcelPM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("TariffVersionUploadedExcel", entityPM.Tenant); 
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(TariffVersionUploadedExcelPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 