 
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
   public partial class ClaimImporterDeclarsPage3AUpdateService:EntityUpdateService<ClaimImporterDeclarsPage3A,ClaimImporterDeclarsPage3APM,ClaimPM>
   {
   
        ClaimImporterDeclarsPage3ARepository entityRepository;
        public ClaimImporterDeclarsPage3AUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            context = context ??mainContext as ICustomContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new ClaimImporterDeclarsPage3ADataMapping();
            Repository = new ClaimImporterDeclarsPage3ARepository(context);
        }

       
        private ICustomContext currentContext;
        public ClaimImporterDeclarsPage3AUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimImporterDeclarsPage3AUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(ClaimImporterDeclarsPage3APM entityPM)
        {
            ClaimImporterDeclarsPage3AKeys entityKeys = new ClaimImporterDeclarsPage3AKeys() { ClaimId = entityPM.ClaimId, LineNo = entityPM.LineNo };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(ClaimImporterDeclarsPage3APM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(ClaimImporterDeclarsPage3APM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 