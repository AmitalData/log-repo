 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityDataMappings;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityUpdateServices
{ 
   public partial class PriorityUpdateService:EntityUpdateService<Priority,PriorityPM,EntityPM>
   {
   
        PriorityRepository entityRepository;
        public PriorityUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICRMContext  context = mainContext as CRMContext;
            Mapping = new PriorityDataMapping();
            Repository = new PriorityRepository(context);
        }

       
        private ICRMContext currentContext;
        public PriorityUpdateService(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public PriorityUpdateService(ICRMContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(PriorityPM entityPM)
        {
            PriorityKeys entityKeys = new PriorityKeys() { Code = entityPM.Code };
            return entityKeys;
        }
		 
	 
   }
   
}
	 