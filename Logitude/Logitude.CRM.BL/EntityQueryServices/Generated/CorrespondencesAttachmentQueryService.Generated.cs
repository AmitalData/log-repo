 
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
using Simplog.Server.Infrastructure;
namespace Logitude.CRM.BL.EntityQueryServices
{ 
   public partial class CorrespondencesAttachmentQueryService: EntityQueryService<CorrespondencesAttachment,CorrespondencesAttachmentKeys,CorrespondencesAttachmentPM,object,CorrespondencesAttachmentKeys>
   {
   
        CorrespondencesAttachmentRepository repository;
		ICRMContext  context;
        public CorrespondencesAttachmentQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new CorrespondencesAttachmentRepository(context);
            Repository = repository;
            mapping = new CorrespondencesAttachmentDataMapping();
        }

        public CorrespondencesAttachmentQueryService(CorrespondencesAttachmentRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CorrespondencesAttachmentDataMapping();
        }

        public CorrespondencesAttachmentQueryService(ICRMContext context)
        {
            this.repository = new CorrespondencesAttachmentRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CorrespondencesAttachmentDataMapping();
        }
		 
		public  CorrespondencesAttachmentPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CorrespondencesAttachmentKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CorrespondencesAttachment entityPOCO)
        {
            CorrespondencesAttachmentKeys entityKeys = new CorrespondencesAttachmentKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 