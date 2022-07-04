 
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
   public partial class ExportStorageUpdateService:EntityUpdateService<ExportStorage,ExportStoragePM,EntityPM>
   {
   
        ExportStorageRepository entityRepository;
        public ExportStorageUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            context = context ??mainContext as ICustomContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new ExportStorageDataMapping();
            Repository = new ExportStorageRepository(context);
        }

       
        private ICustomContext currentContext;
        public ExportStorageUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ExportStorageUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(ExportStoragePM entityPM)
        {
            ExportStorageKeys entityKeys = new ExportStorageKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(ExportStoragePM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("ExportStorage", entityPM.Tenant); 
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(ExportStoragePM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 