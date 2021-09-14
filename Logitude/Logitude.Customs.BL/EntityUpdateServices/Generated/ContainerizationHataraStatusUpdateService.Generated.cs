 
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
   public partial class ContainerizationHatataStatusUpdateService:EntityUpdateService<ContainerizationHatataStatus,ContainerizationHatataStatusPM,EntityPM>
   {
   
        ContainerizationHatataStatusRepository entityRepository;
        public ContainerizationHatataStatusUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            context = context ??mainContext as ICustomContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new ContainerizationHatataStatusDataMapping();
            Repository = new ContainerizationHatataStatusRepository(context);
        }

       
        private ICustomContext currentContext;
        public ContainerizationHatataStatusUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ContainerizationHatataStatusUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(ContainerizationHatataStatusPM entityPM)
        {
            ContainerizationHatataStatusKeys entityKeys = new ContainerizationHatataStatusKeys() { Code = entityPM.Code };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(ContainerizationHatataStatusPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(ContainerizationHatataStatusPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 