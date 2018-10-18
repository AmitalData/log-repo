 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class AttachmentTypeQueryService: EntityQueryService<AttachmentType,AttachmentTypeKeys,AttachmentTypePM,object,AttachmentTypeKeys>
   {
   
        AttachmentTypeRepository repository;
		ICustomContext  context;
        public AttachmentTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AttachmentTypeRepository(context);
            Repository = repository;
            mapping = new AttachmentTypeDataMapping();
        }

        public AttachmentTypeQueryService(AttachmentTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AttachmentTypeDataMapping();
        }

        public AttachmentTypeQueryService(ICustomContext context)
        {
            this.repository = new AttachmentTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AttachmentTypeDataMapping();
        }
		 
		public  AttachmentTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AttachmentTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AttachmentType entityPOCO)
        {
            AttachmentTypeKeys entityKeys = new AttachmentTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 