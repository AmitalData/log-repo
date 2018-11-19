 
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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityDataMappings;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityUpdateServices
{ 
   public partial class CorrespondencesAttachmentUpdateService:EntityUpdateService<CorrespondencesAttachment,CorrespondencesAttachmentPM,EntityPM>
   {
   
        CorrespondencesAttachmentRepository entityRepository;
        public CorrespondencesAttachmentUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICRMContext  context = mainContext as CRMContext;
            context = context ??mainContext as ICRMContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new CorrespondencesAttachmentDataMapping();
            Repository = new CorrespondencesAttachmentRepository(context);
        }

       
        private ICRMContext currentContext;
        public CorrespondencesAttachmentUpdateService(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public CorrespondencesAttachmentUpdateService(ICRMContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(CorrespondencesAttachmentPM entityPM)
        {
            CorrespondencesAttachmentKeys entityKeys = new CorrespondencesAttachmentKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
		protected override void FillDefaultValuesOnCreate(CorrespondencesAttachmentPM entityPM)
        {     
  
		
		    entityPM.Id = IdCounter.GetNumber("CorrespondencesAttachment", entityPM.Tenant); 
					
	    }
        
		protected override void FillDefaultValuesOnUpdate(CorrespondencesAttachmentPM entityPM)
        {       
           
        }
		  
		 
	 
   }
   
}
	 