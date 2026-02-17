 
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
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.BL.EntityDataMappings;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.WarehouseLib.Data.EntityKeys;
using Logitude.WarehouseLib.Data;

namespace Logitude.WarehouseLib.BL.EntityUpdateServices
{ 
   public partial class WarehouseEntryStatusUpdateService:EntityUpdateService<WarehouseEntryStatus,WarehouseEntryStatusPM,EntityPM>
   {
   
        WarehouseEntryStatusRepository entityRepository;
        public WarehouseEntryStatusUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IWarehouseContext  context = mainContext as WarehouseContext;
            context = context ??mainContext as IWarehouseContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new WarehouseEntryStatusDataMapping();
            Repository = new WarehouseEntryStatusRepository(context);
        }

       
        private IWarehouseContext currentContext;
        public WarehouseEntryStatusUpdateService(int tenant)
        {
            currentContext = WarehouseContext.GetContext(tenant);
        }

        public WarehouseEntryStatusUpdateService(IWarehouseContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(WarehouseEntryStatusPM entityPM)
        {
            WarehouseEntryStatusKeys entityKeys = new WarehouseEntryStatusKeys() { Code = entityPM.Code };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(WarehouseEntryStatusPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(WarehouseEntryStatusPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 