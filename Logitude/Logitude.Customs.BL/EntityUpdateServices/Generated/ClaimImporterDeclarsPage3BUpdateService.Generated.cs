 
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
   public partial class ClaimImporterDeclarsPage3BUpdateService:EntityUpdateService<ClaimImporterDeclarsPage3B,ClaimImporterDeclarsPage3BPM,ClaimPM>
   {
   
        ClaimImporterDeclarsPage3BRepository entityRepository;
        public ClaimImporterDeclarsPage3BUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            context = context ??mainContext as ICustomContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new ClaimImporterDeclarsPage3BDataMapping();
            Repository = new ClaimImporterDeclarsPage3BRepository(context);
        }

       
        private ICustomContext currentContext;
        public ClaimImporterDeclarsPage3BUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimImporterDeclarsPage3BUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(ClaimImporterDeclarsPage3BPM entityPM)
        {
            ClaimImporterDeclarsPage3BKeys entityKeys = new ClaimImporterDeclarsPage3BKeys() { ClaimId = entityPM.ClaimId, LineNo = entityPM.LineNo };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(ClaimImporterDeclarsPage3BPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(ClaimImporterDeclarsPage3BPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 